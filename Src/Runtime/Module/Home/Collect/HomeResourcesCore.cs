using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 家园采集资源core
/// </summary>
public abstract class HomeResourcesCore : EntityBaseComponent, ICollectResourceCore
{
    public ulong Id { get; private set; }

    public eResourceType ResourceType => eResourceType.HomeResource;

    public GameObject LogicRoot => RefEntity.EntityRoot;

    public Vector3 Position => RefEntity.Position;

    public bool IsDead { get; private set; }

    public eAction SupportAction { get; set; } = eAction.None;
    public ResourceDataCore Data { get; private set; }

    public int GetActionLevel(eAction action)
    {
        if ((action & SupportAction) != 0)
        {
            return Data.DRHomeResources.Lv;
        }
        else
        {
            Log.Error($"HomeResourcesCore.GetActionLevel() is not implemented action:{action}");
            return 0;
        }
    }

    private HomeSoilResourceRelation _addedSoilResourceRelation;//上次添加关系的组建 方式一个家园到另外家园瞬间的引用关系错误

    protected virtual void Awake()
    {
        IsDead = false;
    }

    protected virtual void Start()
    {
        if (HomeModuleCore.IsInited)//在家园里
        {
            if (Data != null)
            {
                AddSoilResourceRelation();
            }
            else
            {
                Log.Error($"家园采集资源 Data 组件 is null");
            }
        }

        RefEntity.EntityEvent.EntityAttributeUpdate += OnEntityAttributeUpdate;
    }

    protected virtual void OnDestroy()
    {
        if (_addedSoilResourceRelation)
        {
            _addedSoilResourceRelation.RemoveResourceOnSoil((long)Id);
            _addedSoilResourceRelation = null;
        }

        GetComponent<HomeActionProgressData>().EndProgressAction();

        RefEntity.EntityEvent.EntityAttributeUpdate -= OnEntityAttributeUpdate;
    }

    private void OnEntityAttributeUpdate(eAttributeType type, int value)
    {
        if (type == eAttributeType.MaxActionValue)
        {
            if ((PROGRESS_ACTION_MASK & SupportAction) != 0)
            {
                UpdateProgressActionProgress();
            }
        }
    }

    /// <summary>
    /// 初始化基础的ResourceData之后调用  用来第一次初始化基础的内容
    /// </summary>
    public void OnInitedResourceData()
    {
        Id = (ulong)RefEntity.BaseData.Id;
        Data = GetComponent<ResourceDataCore>();
        DRHomeResources dr = Data.DRHomeResources;
        SupportAction = TableUtil.ConvertToBitEnum<eAction>(dr.HomeAction);

        if ((SupportAction & eAction.Pick) != 0)//捡东西很特殊 没有进度但是需要选中显示进度icon 所以只能特殊处理一下 https://linear.app/project-linco/issue/LNCO-4361/砍树挖矿采草时，需要显示名字和提示等信息
        {
            GetComponent<HomeActionProgressData>().StartProgressAction(SupportAction, 1);//最大进度给1点 不要给0点 也加不了进度 所以给1没关系
        }
        else if ((PROGRESS_ACTION_MASK & SupportAction) != 0)
        {
            UpdateProgressActionProgress();
        }
    }

    private void UpdateProgressActionProgress()
    {
        float maxActionValue = RefEntity.EntityAttributeData.GetRealValue(eAttributeType.MaxActionValue);
        GetComponent<HomeActionProgressData>().StartProgressAction(SupportAction, maxActionValue);
    }

    /// <summary>
    /// 添加家园土地资源关系
    /// </summary>
    public void AddSoilResourceRelation()
    {
        if (_addedSoilResourceRelation != null)
        {
            Log.Error($"家园采集资源 {Id} 已经添加过关系了");
            return;
        }

        if (!HomeModuleCore.IsInited)
        {
            Log.Error($"添加家园土地资源关系时家园没有初始化 {Id}");
            return;
        }

        _addedSoilResourceRelation = HomeModuleCore.SoilResourceRelation;
        _addedSoilResourceRelation.AddResourceOnSoil((long)Id, Data.SaveData.Id);
    }

    public bool CheckSupportAction(eAction action)
    {
        if (IsDead)
        {
            return false;
        }

        return (SupportAction & action) != 0;
    }

    public bool CheckPlayerAction(long playerId, eAction action)
    {
        return CheckSupportAction(action);
    }

    public void ExecuteAction(eAction action, int toolCid, long playerId, long entityId, int skillId, object actionData)
    {
        if (!CheckPlayerAction(playerId, action))
        {
            throw new SystemException($"家园采集资源 action {action} not support,isDead:{IsDead}");
        }

        OnExecuteAction(action, playerId, entityId, skillId);

        IsDead = true;
        OnDeath();
    }

    protected virtual void OnExecuteAction(eAction action, long playerId, long entityId, int skillId)
    {
        if (action == eAction.Pick)//捡东西没有进度伤害 也必须给个最大伤害 否则掉落分配有问题
        {
            RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(entityId, int.MaxValue);
        }
    }

    public virtual void ExecuteProgress(eAction targetCurAction, long triggerEntityId, int skillId, int deltaProgress, bool isCrit, bool isPreEffect)
    {
        if (deltaProgress <= 0)
        {
            Log.Error($"家园采集资源进度变化值无效id: {Id} deltaProgress: {deltaProgress}");
            return;
        }

        RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(triggerEntityId, deltaProgress);
    }

    /// <summary>
    /// 当采集资源死亡时
    /// </summary>
    protected virtual void OnDeath()
    {
    }
}