/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 碰撞触发效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SECollisionTriggerCore.cs
* 
*/

using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class SECollisionTriggerCore : SkillEffectBase
{

    protected List<EntityBase> TriggerList;
    protected DRSkill CurSkillCfg;
    protected int[] EffectIDList;
    public override void OnAdd()
    {
        base.OnAdd();
        EffectIDList = EffectCfg.Parameters;
        RefEntity.EntityEvent.EntityTriggerEnter += EntityTriggerEnter;
        TriggerList = new();
        CurSkillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(SkillID);
    }

    public override void OnRemove()
    {
        RefEntity.EntityEvent.EntityTriggerEnter -= EntityTriggerEnter;
        TriggerList = null;
        CurSkillCfg = null;
        EffectIDList = null;
        base.OnRemove();
    }
    protected void EntityTriggerEnter(EntityBase entityBase)
    {
        TriggerList.Add(entityBase);
    }
    public override void Update()
    {
        base.Update();
        UpdateTrigger();
        TriggerList.Clear();
    }

    protected virtual void UpdateTrigger()
    {

    }
    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }
        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        if (!RefEntity.TryGetComponent(out EntityCollisionCore entityCollisionCore))
        {
            Log.Error($"SECollisionTriggerCore not find EntityCollisionCore cpt");
            return;
        }
        if (entityCollisionCore.EntityTriggerSet.Count > 0)
        {
            foreach (long id in entityCollisionCore.EntityTriggerSet)
            {
                if (GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(id, out EntityBase entity))
                {
                    TriggerList.Add(entity);
                }
            }
        }
    }
}