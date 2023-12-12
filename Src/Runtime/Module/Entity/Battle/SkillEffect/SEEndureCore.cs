/*
 * @Author: xiang huan
 * @Date: 2022-09-06 15:36:33
 * @Description: 霸体效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEEndureCore.cs
 * 
 */

public class SEEndureCore : SkillEffectBase
{

    public override void OnAdd()
    {
        base.OnAdd();

        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.AddBattleState(BattleDefine.eBattleState.Endure);
        }
    }

    public override void OnRemove()
    {
        if (RefEntity.BattleDataCore != null)
        {
            RefEntity.BattleDataCore.RemoveBattleState(BattleDefine.eBattleState.Endure);
        }
        base.OnRemove();
    }
}