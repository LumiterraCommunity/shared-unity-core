/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 宠物阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/PetCampDataCore.cs
 * 
 */


using UnityGameFramework.Runtime;


/// <summary>
/// 宠物阵营数据
/// </summary>
public class PetCampDataCore : EntityCampDataCore
{
    private EntityBase _refOwner;
    public override EntityBase RefOwner => GetRefOwner();
    protected PetDataCore PetDataCore;
    private EntityBase GetRefOwner()
    {
        if (_refOwner != null)
        {
            return _refOwner;
        }
        if (RefEntity.TryGetComponent(out PetDataCore PetDataCore))
        {
            if (!GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(PetDataCore.OwnerId, out _refOwner))
            {
                Log.Error("EntityCampDataCore Owner is null");
            };
        }
        return _refOwner ?? RefEntity;
    }
    private void Start()
    {
        if (RefOwner.BaseData.Id != RefEntity.BaseData.Id)
        {
            RefOwner.EntityEvent.ChangeCamp += OnOwnerChangeCamp;
        }
    }

    private void OnDestroy()
    {
        if (RefOwner.BaseData.Id != RefEntity.BaseData.Id)
        {
            RefOwner.EntityEvent.ChangeCamp -= OnOwnerChangeCamp;
        }
    }

    private void OnOwnerChangeCamp()
    {
        RefEntity.EntityEvent.ChangeCamp?.Invoke();
    }
    public override bool CheckIsEnemy(EntityBase other)
    {
        return RefOwner.EntityCampDataCore.CheckIsEnemy(other);
    }
    public override bool CheckIsFriend(EntityBase other)
    {
        return RefOwner.EntityCampDataCore.CheckIsFriend(other);
    }
}