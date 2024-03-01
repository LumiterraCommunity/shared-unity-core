/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 宠物阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/PetCampDataCore.cs
 * 
 */


/// <summary>
/// 宠物阵营数据
/// </summary>
public class PetCampDataCore : EntityCampDataCore
{
    protected PetDataCore PetDataCore;
    private void Start()
    {
        if (RefOwner != null)
        {
            RefOwner.EntityEvent.ChangeCamp += OnOwnerChangeCamp;
        }
    }

    private void OnDestroy()
    {
        if (RefOwner != null)
        {
            RefOwner.EntityEvent.ChangeCamp -= OnOwnerChangeCamp;
        }
    }

    private void OnOwnerChangeCamp()
    {
        RefEntity.EntityEvent.ChangeCamp?.Invoke();
    }

    /// <summary>
    /// 基于主人判断是否是敌人
    /// </summary>
    public override bool IsEnemy(EntityBase other)
    {
        if (RefOwner == null || RefOwner.BaseData.Id == other.BaseData.Id)
        {
            return false;
        }
        return RefOwner.EntityCampDataCore.IsEnemy(other);
    }

    /// <summary>
    /// 基于主人判断是否是友军
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool IsFriend(EntityBase other)
    {
        if (RefOwner == null)
        {
            return false;
        }

        if (RefOwner.BaseData.Id == other.BaseData.Id)
        {
            return true;
        }

        return RefOwner.EntityCampDataCore.IsFriend(other);
    }
}