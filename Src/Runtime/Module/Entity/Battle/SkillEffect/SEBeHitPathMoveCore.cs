/*
 * @Author: xiang huan
 * @Date: 2022-08-12 14:36:36
 * @Description: 受击路径移动效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEBeHitPathMoveCore.cs
 * 
 */


using GameMessageCore;
using UnityGameFramework.Runtime;

public class SEBeHitPathMoveCore : SEPathMoveCore
{
    public override void OnAdd()
    {
        base.OnAdd();
        ChangeBeHitMoveStatus();
    }

    private void ChangeBeHitMoveStatus()
    {

        RefEntity.EntityEvent.EntityBeHitMove?.Invoke(EffectCfg.Duration);

    }


    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        if (targetEntity.BattleDataCore != null)
        {
            //目标方已经死亡
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }

            if (targetEntity.BattleDataCore.HasBattleState(BattleDefine.eBattleState.Endure))
            {
                return false;
            }

        }
        return true;
    }

    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        if (EffectCfg.Parameters == null || EffectCfg.Parameters.Length <= 0)
        {
            Log.Error($"SEPathMove Parameters Error EffectID = {EffectID}");
            return null;
        }
        float distance = EffectCfg.Parameters[0] * MathUtilCore.CM2M;
        UnityEngine.Vector3 curPos = targetEntity.Position;
        UnityEngine.Vector3 moveDir = inputData.Dir;
        moveDir.Set(moveDir.x, 0, moveDir.z);
        UnityEngine.Vector3 targetPos = curPos + (moveDir.normalized * distance);
        DamageEffect effect = new();
        effect.BeatBackValue = new();
        effect.BeatBackValue.CurLoc = NetUtilCore.LocToNet(curPos);
        effect.BeatBackValue.BackToPos = NetUtilCore.LocToNet(targetPos);
        return effect;
    }
}
