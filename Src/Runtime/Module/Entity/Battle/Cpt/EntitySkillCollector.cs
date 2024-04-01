using UnityGameFramework.Runtime;
/// <summary>
/// 实体技能采集基类
/// </summary>
public class EntitySkillCollector : EntityBaseComponent
{
    protected EntitySkillDataCore EntitySkillDataCore;
    protected int LastInHandItemSkillID = -1;//记录上次手持物技能id,用于移除

    protected virtual void Start()
    {
        EntitySkillDataCore = RefEntity.GetComponent<EntitySkillDataCore>();
        //装备
        RefEntity.EntityEvent.EntityAvatarUpdated += OnUpdateEquipmentSkillID;
        //手持物品
        RefEntity.EntityEvent.InHandItemChange += OnInHandItemChanged;
    }

    protected virtual void OnDestroy()
    {
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.EntityAvatarUpdated -= OnUpdateEquipmentSkillID;
            RefEntity.EntityEvent.InHandItemChange -= OnInHandItemChanged;
        }
    }

    protected virtual void OnUpdateEquipmentSkillID()
    {
        //to override
    }

    /// <summary>
    /// 玩家手持物品发生变化
    /// </summary>
    /// <param name="itemID"></param>
    protected virtual void OnInHandItemChanged(int inHandItem)
    {
        if (LastInHandItemSkillID > 0)
        {
            EntitySkillDataCore.RemoveSkillGroupID(eSkillGroupType.InHandItem, LastInHandItemSkillID);//移除上次手持物技能
        }

        if (inHandItem <= 0)
        {
            return;
        }

        DRItem itemCfg = GFEntryCore.DataTable.GetDataTable<DRItem>().GetDataRow(inHandItem);
        if (itemCfg == null)
        {
            Log.Error($"OnUpdateInHandItem not find item id:{inHandItem}");
            return;
        }

        EntitySkillDataCore.AddSkillGroupID(eSkillGroupType.InHandItem, itemCfg.GivenSkillId);
        LastInHandItemSkillID = itemCfg.GivenSkillId;
    }
}