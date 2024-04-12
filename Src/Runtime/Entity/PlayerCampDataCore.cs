/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 玩家阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/PlayerCampDataCore.cs
 * 
 */

/// <summary>
/// 玩家阵营数据
/// </summary>
public class PlayerCampDataCore : EntityCampDataCore
{
    /// <summary>
    /// 是否是敌人
    /// </summary>
    protected override bool IsEnemy(EntityBase otherOwner)
    {
        //检测目标所在区域
        if (otherOwner.TryGetComponent(out EntityBattleArea entityBattleArea))
        {
            //和平区域不可以攻击
            if (entityBattleArea.CurAreaType == eBattleAreaType.Peace)
            {
                return false;
            }

            //混乱区域可以攻击，无需判断阵营
            if (entityBattleArea.CurAreaType == eBattleAreaType.Chaos)
            {
                return true;
            }
        }
        return base.IsEnemy(otherOwner);
    }

    /// <summary>
    /// 是否是友军
    /// </summary>
    protected override bool IsFriend(EntityBase otherOwner)
    {
        //检测目标所在区域
        if (otherOwner.TryGetComponent(out EntityBattleArea entityBattleArea))
        {
            //和平区域算友军
            if (entityBattleArea.CurAreaType == eBattleAreaType.Peace)
            {
                return true;
            }

            //混乱区域算敌军
            if (entityBattleArea.CurAreaType == eBattleAreaType.Chaos)
            {
                return false;
            }
        }
        return base.IsEnemy(otherOwner);
    }
}