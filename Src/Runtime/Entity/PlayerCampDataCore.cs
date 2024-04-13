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

        if (RefEntity.TryGetComponent(out EntityBattleArea myBattleArea) && otherOwner.TryGetComponent(out EntityBattleArea otherBattleArea))
        {
            //我或者目标在和平区域
            if (myBattleArea.CurAreaType == eBattleAreaType.Peace || otherBattleArea.CurAreaType == eBattleAreaType.Peace)
            {
                return false;
            }
            //目标在混乱区域
            if (otherBattleArea.CurAreaType == eBattleAreaType.Chaos)
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
        if (RefEntity.TryGetComponent(out EntityBattleArea myBattleArea) && otherOwner.TryGetComponent(out EntityBattleArea otherBattleArea))
        {
            //我或者目标在和平区域
            if (myBattleArea.CurAreaType == eBattleAreaType.Peace || otherBattleArea.CurAreaType == eBattleAreaType.Peace)
            {
                return true;
            }

            //混乱区域算敌军
            if (otherBattleArea.CurAreaType == eBattleAreaType.Chaos)
            {
                return false;
            }
        }
        return base.IsFriend(otherOwner);
    }
}