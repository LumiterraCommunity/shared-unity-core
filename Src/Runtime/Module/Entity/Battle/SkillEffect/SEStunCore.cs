/// <summary>
/// 眩晕技能效果球
/// </summary>
public class SEStunCore : SkillEffectBase
{
    public override void OnAdd()
    {
        base.OnAdd();

        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.AddBattleState(BattleDefine.eBattleState.Stun);
        }

        RefEntity.EntityEvent.EntityReceiveStunEffect?.Invoke();
    }

    public override void OnRemove()
    {
        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.RemoveBattleState(BattleDefine.eBattleState.Stun);
        }

        RefEntity.EntityEvent.EntityRemoveStunEffect?.Invoke();
        base.OnRemove();
    }

    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        if (targetEntity.BattleDataCore.HasBattleState(BattleDefine.eBattleState.Endure))
        {
            return false;//霸体状态不添加眩晕效果
        }

        return base.CheckApplyEffect(fromEntity, targetEntity);
    }
}