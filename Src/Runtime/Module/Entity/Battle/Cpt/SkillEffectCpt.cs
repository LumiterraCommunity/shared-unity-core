/*
 * @Author: xiang huan
 * @Date: 2022-07-19 13:38:00
 * @Description: 技能效果组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/SkillEffectCpt.cs
 * 
 */
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityGameFramework.Runtime;

public class SkillEffectCpt : EntityBaseComponent
{

    public Dictionary<eSEStatusType, List<SkillEffectBase>> SkillEffectMap { get; private set; }
    private readonly Queue<SkillEffectBase> _removeEffectQueue = new(); //需要移除的效果队列
    private int _immuneFlag; //免疫标识
    private bool _isNetDirty = true;
    private string _netSaveData;

    private void Awake()
    {
        SkillEffectMap = new()
        {
            { eSEStatusType.Runtime, new() },
            { eSEStatusType.Static, new() },
            { eSEStatusType.StaticUpdate, new() }
        };
    }

    private void Start()
    {
        RefEntity.EntityEvent.EntitySkillEffectLayerUpdate += OnSkillEffectUpdate;
        RefEntity.EntityEvent.EntitySkillEffectIntervalTimeUpdate += OnSkillEffectUpdate;
        _isNetDirty = true;
    }
    private void OnDestroy()
    {
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.EntitySkillEffectLayerUpdate -= OnSkillEffectUpdate;
            RefEntity.EntityEvent.EntitySkillEffectIntervalTimeUpdate -= OnSkillEffectUpdate;
        }
        ClearAllSkillEffect();
    }

    private void OnSkillEffectUpdate(int effect)
    {
        OnSeListUpdated(false);
    }
    private void Update()
    {
        List<SkillEffectBase> runList = SkillEffectMap[eSEStatusType.Runtime];
        if (runList.Count > 0)
        {
            long curTimeStamp = TimeUtil.GetServerTimeStamp();
            for (int i = runList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = runList[i];
                if ((effect.DestroyTimestamp > 0 && curTimeStamp >= effect.DestroyTimestamp) || !RefEntity.BattleDataCore.IsLive())
                {
                    _removeEffectQueue.Enqueue(runList[i]);
                }
                else
                {
                    effect.Update();
                }
            }
            UpdateRemoveEffectQueue();
        }
        //静态需要刷新的
        List<SkillEffectBase> staticUpdateList = SkillEffectMap[eSEStatusType.StaticUpdate];
        if (staticUpdateList.Count > 0)
        {
            for (int i = staticUpdateList.Count - 1; i >= 0; i--)
            {
                staticUpdateList[i].Update();
            }
        }
    }
    public void InitEffectData(string effectData)
    {
        if (string.IsNullOrEmpty(effectData))
        {
            return;
        }
        SkillEffectSaveDataConfig config = JsonConvert.DeserializeObject<SkillEffectSaveDataConfig>(effectData);
        InitRuntimeEffectData(config.RunTimeList);
        InitStaticEffectData(config.StaticList);
        InitStaticEffectData(config.StaticUpdateList);
    }

    public void InitRuntimeEffectData(List<SkillEffectSaveData> listData)
    {
        if (listData.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < listData.Count; i++)
        {
            SkillEffectSaveData saveData = listData[i];
            DRSkillEffect skillEffectCfg = GFEntryCore.DataTable.GetDataTable<DRSkillEffect>().GetDataRow(saveData.EffectID);
            if (skillEffectCfg == null)
            {
                Log.Error($"InitSkillEffectConfig not find skill effect skillID = {saveData.SkillID} effectID = {saveData.EffectID}");
                continue;
            }
            SkillEffectBase skillBase = GFEntryCore.SkillEffectFactory.CreateOneSkillEffect(saveData.SkillID, saveData.EffectID, saveData.FromID, RefEntity.BaseData.Id, skillEffectCfg.Duration, saveData.CurLayer, saveData.NextIntervalTime);
            skillBase.DestroyTimestamp = saveData.DestroyTimestamp;
            AddEffectList(skillBase, SkillEffectMap[eSEStatusType.Runtime]);
        }
    }

    public void InitStaticEffectData(List<SkillEffectSaveData> listData)
    {
        if (listData.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < listData.Count; i++)
        {
            SkillEffectSaveData saveData = listData[i];
            SkillEffectBase skillBase = GetEffect(saveData.EffectID, saveData.FromID);
            if (skillBase != null)
            {
                skillBase.SetSaveData(saveData);
            }
        }
    }

    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    /// <param name="effect">效果</param>
    public bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity, SkillEffectBase effect)
    {
        if ((_immuneFlag & effect.EffectFlag) != 0)
        {
            return false;
        }
        return effect.CheckApplyEffect(fromEntity, targetEntity);
    }

    //应用某个效果
    public void ApplyOneEffect(SkillEffectBase effect)
    {
        if (effect == null)
        {
            return;
        }
        if (effect.Duration != 0)
        {
            effect.DestroyTimestamp = effect.Duration > 0 ? (TimeUtil.GetServerTimeStamp() + effect.Duration) : -1;
            if (effect.Duration > 0)
            {
                effect.StatusType = eSEStatusType.Runtime;
                AddEffectList(effect, SkillEffectMap[eSEStatusType.Runtime]);
            }
            else
            {
                if (!effect.IsUpdate)
                {
                    effect.StatusType = eSEStatusType.Static;
                    AddEffectList(effect, SkillEffectMap[eSEStatusType.Static]);
                }
                else
                {
                    effect.StatusType = eSEStatusType.StaticUpdate;
                    AddEffectList(effect, SkillEffectMap[eSEStatusType.StaticUpdate]);
                }
            }

        }
        else
        {
            effect.AddEffect(RefEntity);
            effect.Start();
            effect.RemoveEffect();
            effect.Dispose();
        }
    }

    //添加到效果列表
    private void AddEffectList(SkillEffectBase newEffect, List<SkillEffectBase> effectList)
    {
        //找到新效果的相同施法者的相同效果，当效果不允许重复时，覆盖其他施法者的效果
        SkillEffectBase oldEffect = null;
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            //相同效果，不允许重复
            if (effectList[i].EffectID == newEffect.EffectID && !effectList[i].EffectCfg.IsRepeat)
            {
                //相同施法者，进行层数刷新，否则覆盖
                if (effectList[i].FromID == newEffect.FromID)
                {
                    oldEffect = effectList[i];
                    break;
                }
                else
                {
                    _removeEffectQueue.Enqueue(effectList[i]);
                    break;
                }
            }
        }
        UpdateRemoveEffectQueue();
        //相同施法者的相同buff，刷新层数
        if (oldEffect != null)
        {
            //刷新层级
            if (oldEffect.CurLayer < oldEffect.EffectCfg.MaxLayer)
            {
                oldEffect.UpdateLayer(oldEffect.CurLayer + 1);
            }
            oldEffect.DestroyTimestamp = newEffect.DestroyTimestamp;
            newEffect.Dispose();
        }
        else
        {
            newEffect.AddEffect(RefEntity);
            newEffect.Start();
            effectList.Add(newEffect);
        }
        OnSeListUpdated();
    }

    //取消某种效果
    public void AbolishSkillEffect(int effectID)
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                if (effect.EffectID == effectID)
                {
                    _removeEffectQueue.Enqueue(effectList[i]);
                }
            }
        }
        UpdateRemoveEffectQueue();
    }

    //取消某种效果
    public void AbolishSkillEffect(int effectID, long fromID)
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                if (effect.EffectID == effectID && effect.FromID == fromID)
                {
                    _removeEffectQueue.Enqueue(effectList[i]);
                }
            }
        }
        UpdateRemoveEffectQueue();
    }
    public void ClearAllSkillEffect()
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                _removeEffectQueue.Enqueue(effectList[i]);
            }
        }

        UpdateRemoveEffectQueue();
    }

    public void OnSeListUpdated(bool isUpdateImmuneFlag = true)
    {
        if (isUpdateImmuneFlag)
        {
            UpdateImmuneFlag();
        }
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.SeListUpdated?.Invoke();
        }
        _isNetDirty = true;
    }

    private void UpdateImmuneFlag()
    {
        _immuneFlag = 0;
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                _immuneFlag |= effect.EffectImmuneFlag;
            }
        }
    }

    //获取效果
    public SkillEffectBase GetEffect(int effectID)
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                if (effect.EffectID == effectID)
                {
                    return effect;
                }
            }
        }
        return null;
    }

    //获取效果
    public SkillEffectBase GetEffect(int effectID, long fromID)
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                if (effect.EffectID == effectID && effect.FromID == fromID)
                {
                    return effect;
                }
            }
        }
        return null;
    }
    //获取效果
    public SkillEffectBase GetEffectByType(int effectType)
    {
        foreach (KeyValuePair<eSEStatusType, List<SkillEffectBase>> item in SkillEffectMap)
        {
            List<SkillEffectBase> effectList = item.Value;
            for (int i = effectList.Count - 1; i >= 0; i--)
            {
                SkillEffectBase effect = effectList[i];
                if (effect.EffectCfg.EffectType == effectType)
                {
                    return effect;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获取运行效果的保存数据
    /// </summary>
    /// <returns></returns>
    public string GetNetData()
    {
        if (_isNetDirty)
        {
            SkillEffectSaveDataConfig config = new()
            {
                RunTimeList = GetSkillEffectSaveDataList(eSEStatusType.Runtime, true),
                StaticList = GetSkillEffectSaveDataList(eSEStatusType.Static, false),
                StaticUpdateList = GetSkillEffectSaveDataList(eSEStatusType.StaticUpdate, false)
            };
            _netSaveData = config.ToJson();
            _isNetDirty = false;
        }

        return _netSaveData;
    }

    private List<SkillEffectSaveData> GetSkillEffectSaveDataList(eSEStatusType statusType, bool isForce)
    {
        List<SkillEffectSaveData> saveDataList = new();
        List<SkillEffectBase> effectList = SkillEffectMap[statusType];
        for (int i = 0; i < effectList.Count; i++)
        {
            if (isForce || effectList[i].IsStaticSync)
            {
                saveDataList.Add(effectList[i].GetSaveData());
            }
        }
        return saveDataList;
    }

    private void UpdateRemoveEffectQueue()
    {
        if (_removeEffectQueue.Count <= 0)
        {
            return;
        }

        while (_removeEffectQueue.TryDequeue(out SkillEffectBase effect))
        {
            List<SkillEffectBase> effectList = SkillEffectMap[effect.StatusType];
            _ = effectList.Remove(effect);

            effect.RemoveEffect();
            effect.Dispose();
        }
        OnSeListUpdated();
    }
}
