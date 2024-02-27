using UnityGameFramework.Runtime;


/// <summary>
/// 宠物核心数据
/// 家园宠物和跟随宠物都使用这个组件，不过跟随宠物用到的是这个组件的子类FollowingPetDataCore
/// </summary>
public class PetDataCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物所有特性集合,通过位存储
    /// </summary>
    public ePetAbility AllAbility { get; protected set; } = ePetAbility.None;
    /// <summary>
    /// 宠物配置
    /// </summary>
    public DRPet PetCfg { protected set; get; }
    /// <summary>
    /// 宠物配置
    /// </summary>
    public int PetCfgId => PetCfg == null ? -1 : PetCfg.Id;
    /// <summary>
    /// 当前手持物id
    /// 用于播种，喂食等操作，使用道具丢炸弹等操作
    /// </summary>
    public int InHandItem { get; protected set; } = 0;

    public void SetPetCfgId(int cfgID)
    {
        PetCfg = GFEntryCore.DataTable.GetDataTable<DRPet>().GetDataRow(cfgID);

        if (PetCfg == null)
        {
            Log.Error($"Can not find pet cfg id:{cfgID}");
        }
    }

    /// <summary>
    /// 设置宠物所有特性
    /// </summary>
    /// <param name="abilities"></param>
    public void SetPetAbility(ePetAbility abilities)
    {
        AllAbility = abilities;
    }

    /// <summary>
    /// 添加宠物特性
    /// </summary>
    /// <param name="ability"></param>
    public void AddPetAbility(ePetAbility ability)
    {
        AllAbility |= ability;
    }

    /// <summary>
    /// 移除宠物特性
    /// </summary>
    /// <param name="ability"></param>
    public void RemovePetAbility(ePetAbility ability)
    {
        AllAbility &= ~ability;
    }

    /// <summary>
    /// 是否有某个宠物特性
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public bool HasPetAbility(ePetAbility ability)
    {
        return (AllAbility & ability) != 0;
    }

    public void SetInHandItem(int cid)
    {
        if (cid == InHandItem)
        {
            return;
        }

        InHandItem = cid;
        RefEntity.EntityEvent.InHandItemChange?.Invoke(cid);
    }
}