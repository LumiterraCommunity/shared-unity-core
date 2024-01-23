/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 玩家阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/PlayerCampDataCore.cs
 * 
 */

using System.Collections.Generic;


/// <summary>
/// 玩家阵营数据
/// </summary>
public class PlayerCampDataCore : EntityCampDataCore
{
    /// <summary>
    /// 是否是敌人
    /// </summary>
    public override bool IsEnemy(EntityBase other)
    {
        //检测目标所在区域
        if (other.TryGetComponent(out EntityBattleArea entityBattleArea))
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
        return base.IsEnemy(other);
    }
}