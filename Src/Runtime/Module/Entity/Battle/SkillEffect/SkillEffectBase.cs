/*
 * @Author: xiang huan
 * @Date: 2022-07-19 10:08:06
 * @Description: 技能效果球基础, 用了引用池，记住继承Clear清除数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SkillEffectBase.cs
 * 
 */
using System;
using GameFramework;
using GameMessageCore;
using UnityGameFramework.Runtime;

public class SkillEffectBase : IReference
{
    /// <summary>
    /// 持续时间 小于零代表一致持续  0代表立即执行销毁  大于0即到时自动销毁 单位ms
    /// </summary>
    public int Duration { get; private set; }

    /// <summary>
    /// 技能释放者ID
    /// </summary>
    public long FromID { get; private set; }

    /// <summary>
    /// 技能接受者ID
    /// </summary>
    public long TargetID { get; private set; }

    /// <summary>
    /// 效果销毁时间 ms
    /// </summary>
    public long DestroyTimestamp { get; set; }
    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillID { get; private set; }
    /// <summary>
    /// 效果ID
    /// </summary>
    public int EffectID { get; private set; }
    /// <summary>
    /// 效果配置
    /// </summary>
    public DRSkillEffect EffectCfg { get; private set; }
    /// <summary>
    /// 效果标识
    /// </summary>
    public int EffectFlag { get; private set; }
    /// <summary>
    /// 效果免疫标识
    /// </summary>
    public int EffectImmuneFlag { get; private set; }

    /// <summary>
    /// 当前层级
    /// </summary>
    public int CurLayer { get; private set; }

    /// <summary>
    /// 需要调用Update, 记得重写为true
    /// </summary>
    public virtual bool IsUpdate => false;

    /// <summary>
    /// 当效果球为静态时，也需要同步数据时，重写为true
    /// </summary>
    public virtual bool IsStaticSync => false;
    /// <summary>
    /// 效果数据
    /// </summary>
    public DamageEffect EffectData { get; private set; }

    /// <summary>
    /// 宿主对象
    /// </summary>
    public EntityBase RefEntity { get; private set; }

    /// <summary>
    /// 下次间隔触发时间
    /// </summary>
    public long NextIntervalTime { get; private set; }
    /// <summary>
    /// 技能输入数据
    /// </summary>
    public InputSkillReleaseData InputData { get; private set; }

    /// <summary>
    ///  是否是添加
    /// </summary>
    /// <value></value>
    public bool IsAdd { get; private set; }

    /// <summary>
    /// 状态类型
    /// </summary> 

    public eSEStatusType StatusType;

    /// <summary>
    /// 设置效果数据
    /// </summary>
    /// <param name="skillID">技能ID</param>
    /// <param name="效果ID">是否循环</param>
    /// <param name="fromID">技能释放者ID</param>
    /// <param name="targetID">技能接受者ID</param>
    /// <param name="duration">持续时间</param>
    /// <param name="curLayer">当前层级</param>
    public virtual void SetData(int skillID, DRSkillEffect effectCfg, long fromID, long targetID, int duration, int curLayer = 1, long nextIntervalTime = 0)
    {
        SkillID = skillID;
        EffectID = effectCfg.Id;
        EffectCfg = effectCfg;
        FromID = fromID;
        TargetID = targetID;
        Duration = duration;
        CurLayer = curLayer;
        EffectFlag = 0;
        NextIntervalTime = nextIntervalTime;
        if (effectCfg.EffectFlag.Length > 0)
        {
            for (int i = 0; i < effectCfg.EffectFlag.Length; i++)
            {
                EffectFlag |= 1 << effectCfg.EffectFlag[i];
            }
        }
        EffectImmuneFlag = 0;
        if (effectCfg.EffectImmuneFlag.Length > 0)
        {
            for (int i = 0; i < effectCfg.EffectImmuneFlag.Length; i++)
            {
                EffectImmuneFlag |= 1 << effectCfg.EffectImmuneFlag[i];
            }
        }
    }


    /// <summary>
    /// 设置效果数据
    /// </summary>
    /// <param name="data"效果数据</param>
    public virtual void SetEffectData(DamageEffect data)
    {
        EffectData = data;
    }

    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public virtual bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        return true;
    }

    /// <summary>
    /// 创建技能效果数据 子类复写
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    /// <param name="inputData">技能输入数据</param>
    public virtual DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        return new DamageEffect();//现在网络传递时通过DamageEffect传递 id 这个不能为空
    }

    public virtual void Clear()
    {
        Duration = 0;
        FromID = 0;
        TargetID = 0;
        DestroyTimestamp = 0;
        EffectID = 0;
        EffectCfg = null;
        SkillID = 0;
        RefEntity = null;
        EffectData = null;
        EffectFlag = 0;
        EffectImmuneFlag = 0;
        CurLayer = 1;
        NextIntervalTime = 0;
        InputData = null;
        IsAdd = false;
        StatusType = eSEStatusType.Runtime;
    }
    /// <summary>
    /// 添加后执行第一次
    /// </summary>
    public virtual void Start()
    {
        UpdateLayer(CurLayer);
    }
    /// <summary>
    /// 刷新效果
    /// </summary>
    public virtual void Update()
    {
        //更新触发间隔
        if (EffectCfg.EffectInterval > 0)
        {
            long curTimeStamp = TimeUtil.GetServerTimeStamp();
            if (curTimeStamp >= NextIntervalTime)
            {
                UpdateIntervalTime(curTimeStamp + EffectCfg.EffectInterval);
                OnInterval();
            }
        }
    }
    /// <summary>
    /// 间隔触发
    /// </summary>
    public virtual void OnInterval()
    {

    }
    /// <summary>
    /// 效果被添加
    /// </summary>
    public virtual void OnAdd()
    {
    }

    /// <summary>
    /// 效果被移除
    /// </summary>
    public virtual void OnRemove()
    {

    }

    /// <summary>
    /// 刷新当前层级
    /// </summary>
    public virtual void UpdateLayer(int layer)
    {
        if (CurLayer == layer)
        {
            return;
        }
        CurLayer = layer;
        RefEntity.EntityEvent.EntitySkillEffectLayerUpdate?.Invoke(EffectID);
    }

    /// <summary>
    /// 刷新间隔触发时间
    /// </summary>
    public virtual void UpdateIntervalTime(long time)
    {
        if (NextIntervalTime == time)
        {
            return;
        }
        NextIntervalTime = time;
        RefEntity.EntityEvent.EntitySkillEffectIntervalTimeUpdate?.Invoke(EffectID);
    }
    //添加效果
    public void AddEffect(EntityBase owner)
    {
        if (IsAdd)
        {
            return;
        }
        RefEntity = owner;
        IsAdd = true;
        OnAdd();
        try
        {
            RefEntity.EntityEvent.EntitySkillEffectAdd?.Invoke(EffectID);
        }
        catch (System.Exception e)
        {
            Log.Error($"AddEffect Error {e}");
        }
    }

    //删除效果
    public void RemoveEffect()
    {
        if (!IsAdd)
        {
            return;
        }
        try
        {
            RefEntity.EntityEvent.EntitySkillEffectRemove?.Invoke(EffectID);
        }
        catch (System.Exception e)
        {
            Log.Error($"RemoveEffect Error {e}");
        }

        OnRemove();
        IsAdd = false;
        RefEntity = null;
    }

    public static SkillEffectBase Create(Type skillEffectClass)
    {
        SkillEffectBase effect = ReferencePool.Acquire(skillEffectClass) as SkillEffectBase;
        return effect;
    }

    /// <summary>
    /// 效果被销毁
    /// </summary>
    public void Dispose()
    {
        RemoveEffect();
        ReferencePool.Release(this);
    }

    /// <summary>
    /// 设置存储数据
    /// </summary>
    public virtual void SetSaveData(SkillEffectSaveData saveData)
    {
        UpdateLayer(saveData.CurLayer);
        UpdateIntervalTime(saveData.NextIntervalTime);
    }

    /// <summary>
    /// 获取存储数据
    /// </summary>
    public SkillEffectSaveData GetSaveData()
    {
        SkillEffectSaveData data = new()
        {
            FromID = FromID,
            DestroyTimestamp = DestroyTimestamp,
            SkillID = SkillID,
            EffectID = EffectID,
            CurLayer = CurLayer,
            NextIntervalTime = NextIntervalTime,
        };
        return data;
    }

    public virtual bool IsShowBuffIcon()
    {
        if (EffectCfg == null)
        {
            return false;
        }

        return EffectCfg.ShowBuffIcon && !string.IsNullOrEmpty(EffectCfg.BuffIcon);
    }

    public virtual void SetInputData(InputSkillReleaseData data)
    {
        InputData = data;
    }

    /// <summary>
    /// 播放预表现
    /// </summary>
    public void PlayPreEffect(EntityBase owner)
    {
        RefEntity = owner;
        OnPlayPreEffect();
    }
    protected virtual void OnPlayPreEffect()
    {

    }
}