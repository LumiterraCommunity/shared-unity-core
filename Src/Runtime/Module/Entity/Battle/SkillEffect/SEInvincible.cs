/** 
 * @Author XQ
 * @Date 2022-08-11 22:43:11
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEInvincible.cs
 */

/// <summary>
/// 无敌效果
/// </summary>
public class SEInvincible : SkillEffectBase
{

    public override void OnAdd()
    {
        base.OnAdd();

        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.AddBattleState(BattleDefine.eBattleState.Invincible);
        }
    }

    public override void OnRemove()
    {
        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.RemoveBattleState(BattleDefine.eBattleState.Invincible);
        }

        base.OnRemove();
    }
}