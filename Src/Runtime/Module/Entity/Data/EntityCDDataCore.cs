/*
 * @Author: xiang huan
 * @Date: 2022-08-09 14:10:48
 * @Description: 实体CD数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/EntityCDDataCore.cs
 * 
 */
using System.Collections.Generic;
public class EntityCDDataCore : EntityBaseComponent
{

    public Dictionary<eEntityCDType, EntityCDBase> EntityCDMap { get; private set; }
    private EntityCDBase _skillEntityCD; //快速调用
    protected GameMessageCore.EntityCD NetEntityCD;
    protected bool IsNetDirty = true;
    protected virtual void Awake()
    {
        EntityCDMap = new();
        NetEntityCD = new();
        _skillEntityCD = EntityCDBase.Create(typeof(EntitySkillCD));
        EntityCDMap.Add(eEntityCDType.Skill, _skillEntityCD); //技能CD
        EntityCDMap.Add(eEntityCDType.Extend, EntityCDBase.Create(typeof(EntityExtendCD))); //扩展CD
    }

    /// <summary>
    /// 初始化实体CD
    /// </summary>
    public void InitSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {
        foreach (KeyValuePair<eEntityCDType, EntityCDBase> item in EntityCDMap)
        {
            item.Value.InitSvrEntityCD(entityCD);
        }
    }
    /// <summary>
    /// 转换成协议EntityCD格式
    /// </summary>
    public GameMessageCore.EntityCD ToSvrEntityCD()
    {
        GameMessageCore.EntityCD entityCD = new();
        foreach (KeyValuePair<eEntityCDType, EntityCDBase> item in EntityCDMap)
        {
            _ = item.Value.ToSvrEntityCD(entityCD);
        }
        return entityCD;
    }

    /// <summary>
    /// 是否技能CD
    /// </summary>
    /// <param name="skillID">技能ID</param>

    public bool IsSkillCD(int skillID)
    {
        return _skillEntityCD.IsCD(skillID);
    }

    /// <summary>
    /// 重置技能CD
    /// </summary>
    /// <param name="skillID">技能ID</param>
    public void ResetSkillCD(int skillID)
    {
        IsNetDirty = true;
        long curTimeStamp = TimeUtil.GetServerTimeStamp();
        long skillCD = SkillUtil.CalculateSkillCD(skillID, RefEntity);
        long cdTime = curTimeStamp + skillCD;
        _skillEntityCD.SetCD(skillID, cdTime);
    }

    /// <summary>
    /// 设置CD
    /// </summary>
    /// <param name="cdType">cd类型</param>
    /// <param name="key">cd key</param>
    /// <param name="time">到期时间戳</param>
    public void SetCD(eEntityCDType cdType, int key, long time)
    {
        IsNetDirty = true;
        if (EntityCDMap.TryGetValue(cdType, out EntityCDBase entityCD))
        {
            entityCD.SetCD(key, time);
        }
    }

    /// <summary>
    /// 是否CD中
    /// </summary>
    /// <param name="cdType">cd类型</param>
    /// <param name="key">cd key</param>
    public bool IsCD(eEntityCDType cdType, int key)
    {
        if (EntityCDMap.TryGetValue(cdType, out EntityCDBase entityCD))
        {
            return entityCD.IsCD(key);
        }
        return false;
    }
    /// <summary>
    /// 获得CD
    /// </summary>
    /// <param name="cdType">cd类型</param>
    /// <param name="key">cd key</param>
    /// <returns>到期时间戳</returns>
    public long GetCD(eEntityCDType cdType, int key)
    {
        if (EntityCDMap.TryGetValue(cdType, out EntityCDBase entityCD))
        {
            return entityCD.GetCD(key);
        }
        return 0;
    }

    /// <summary>
    /// 重置CD
    /// </summary>
    /// <param name="cdType">cd类型</param>
    public void ResetCD(eEntityCDType cdType)
    {
        if (EntityCDMap.TryGetValue(cdType, out EntityCDBase entityCD))
        {
            entityCD.ResetAllCD();
        }
        IsNetDirty = true;
    }

    /// <summary>
    /// 重置所有CD
    /// </summary>
    public void ResetAllCD()
    {
        foreach (KeyValuePair<eEntityCDType, EntityCDBase> item in EntityCDMap)
        {
            item.Value.ResetAllCD();
        }
        IsNetDirty = true;
    }

    public GameMessageCore.EntityCD GetNetData()
    {
        if (IsNetDirty)
        {
            IsNetDirty = false;
            NetEntityCD = ToSvrEntityCD();
        }
        return NetEntityCD;
    }
    private void OnDestroy()
    {
        foreach (KeyValuePair<eEntityCDType, EntityCDBase> item in EntityCDMap)
        {
            item.Value.Dispose();
        }
        EntityCDMap.Clear();
    }
}