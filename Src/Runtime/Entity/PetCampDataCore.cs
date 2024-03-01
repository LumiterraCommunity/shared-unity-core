/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 宠物阵营数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/PetCampDataCore.cs
 * 
 */

/// <summary>
/// 宠物阵营数据
/// </summary>
public class PetCampDataCore : EntityCampDataCore
{
    protected EntityBase Owner; //主人
    protected PetDataCore PetDataCore;
    private void Start()
    {
        PetDataCore = RefEntity.GetComponent<PetDataCore>();
        if (GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(PetDataCore.OwnerId, out EntityBase entity))
        {
            Owner = entity;
        }

    }
    /// <summary>
    /// 是否是敌人
    /// </summary>
    public override bool IsEnemy(EntityBase other)
    {
        if (Owner == null || Owner.BaseData.Id == other.BaseData.Id)
        {
            return false;
        }

        return Owner.EntityCampDataCore.IsEnemy(other);
    }

    /// <summary>
    /// 是否是友军
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool IsFriend(EntityBase other)
    {
        if (Owner == null)
        {
            return false;
        }

        if (Owner.BaseData.Id == other.BaseData.Id)
        {
            return true;
        }

        return Owner.EntityCampDataCore.IsFriend(other);
    }
}