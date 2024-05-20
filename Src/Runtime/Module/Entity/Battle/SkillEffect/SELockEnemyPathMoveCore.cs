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
        eLockPathMoveDirType dir = eLockPathMoveDirType.Forward;
        if (EffectCfg.Parameters.Length > 3)
        {
            dir = (eLockPathMoveDirType)EffectCfg.Parameters[3];
        }
        eLockPathMoveTargetType moveType = eLockPathMoveTargetType.InputTarget;
        if (EffectCfg.Parameters.Length > 4)
        {
            moveType = (eLockPathMoveTargetType)EffectCfg.Parameters[4];
        }

        //计算目标位置和移动方向
        UnityEngine.Vector3 targetPos;
        UnityEngine.Vector3 moveDir;
        if (moveType == eLockPathMoveTargetType.InputTarget)
        {
            moveDir = inputData.Dir * (int)dir;
            if (inputData.TargetPosList != null && inputData.TargetPosList.Length > 0)
            {
                targetPos = inputData.TargetPosList[0];
            }
            else
            {
                //没有目标位置就向前移动
                targetPos = targetEntity.Position + (moveDir.normalized * (minDist * 2));
            }

        }
        else
        {
            targetPos = fromEntity.Position;
            moveDir = (fromEntity.Position - targetEntity.Position) * (int)dir;
        }

        //计算移动距离
        float moveDist = 0;
        if (dir == eLockPathMoveDirType.Forward)
        {
            //靠拢
            float dist = UnityEngine.Vector3.Distance(targetEntity.Position, targetPos);
            if (dist > minDist)
            {
                moveDist = MathF.Min(dist - minDist, maxDist);
            }

        }
        else
        {
            //远离
            float dist = UnityEngine.Vector3.Distance(targetEntity.Position, targetPos);
            if (dist < minDist)
            {
                moveDist = minDist - dist;
            }
        }

        //不做Y轴移动  
        moveDir.Set(moveDir.x, 0, moveDir.z);
        UnityEngine.Vector3 curPos = targetEntity.Position;
        UnityEngine.Vector3 movePos = curPos + (moveDir.normalized * moveDist);
        DamageEffect effect = new()
        {
            BeatBackValue = new()
            {
                CurLoc = NetUtilCore.LocToNet(curPos),
                BackToPos = NetUtilCore.LocToNet(movePos),
                DelayTime = delayTime
            }
        };
        return effect;
    }
}
