using System;
/*
 * @Author: xiang huan
 * @Date: 2022-08-12 14:36:36
 * @Description: 向目标单位移动一段距离
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SELockEnemyPathMoveCore.cs
 * 
 */


using GameMessageCore;
using UnityGameFramework.Runtime;

public class SELockEnemyPathMoveCore : SEPathMoveCore
{
    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        if (EffectCfg.Parameters == null || EffectCfg.Parameters.Length <= 0)
        {
            Log.Error($"SELockEnemyPathMoveCore Parameters Error EffectID = {EffectID}");
            return null;
        }
        float minDist = EffectCfg.Parameters[0] * MathUtilCore.CM2M;
        float maxDist = EffectCfg.Parameters[1] * MathUtilCore.CM2M;
        int delayTime = 0;
        if (EffectCfg.Parameters.Length > 2)
        {
            delayTime = EffectCfg.Parameters[2];
        }
        int dir = 1;
        if (EffectCfg.Parameters.Length > 3)
        {
            dir = EffectCfg.Parameters[3];
        }
        float moveDist = minDist;

        //根据目标位置计算移动距离，无目标位置则移动最小距离
        if (inputData.TargetPosList != null && inputData.TargetPosList.Length > 0)
        {
            float dist = UnityEngine.Vector3.Distance(targetEntity.Position, inputData.TargetPosList[0]);
            if (dist > minDist)
            {
                moveDist = MathF.Min(dist - minDist, maxDist);
            }
            else
            {
                moveDist = 0;
            }
        }
        UnityEngine.Vector3 curPos = targetEntity.Position;
        UnityEngine.Vector3 moveDir = inputData.Dir;
        moveDir.Set(moveDir.x * dir, 0, moveDir.z * dir);
        UnityEngine.Vector3 targetPos = curPos + (moveDir.normalized * moveDist);
        DamageEffect effect = new()
        {
            BeatBackValue = new()
            {
                CurLoc = NetUtilCore.LocToNet(curPos),
                BackToPos = NetUtilCore.LocToNet(targetPos),
                DelayTime = delayTime
            }
        };
        return effect;
    }
}
