/*
 * @Author: xiang huan
 * @Date: 2022-07-19 10:49:14
 * @Description: 技能效果球工厂
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SkillEffectCoreFactory.cs
 * 
 */
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


public class SkillEffectCoreFactory
{
    protected Dictionary<eSkillEffectType, Type> SkillEffectMap;

    /// <summary>
    /// 初始化效果工厂Map
    /// </summary>
    public virtual void InitSkillEffectMap()
    {
        SkillEffectMap = new();
    }
    /// <summary>
    /// 创建技能效果
    /// </summary>
    /// <param name="skillID">技能ID</param>
    /// <param name="effectID">效果类型</param>
    /// <param name="fromID">技能释放者ID</param>
    /// <param name="targetID">技能接收ID</param>
    /// <param name="duration">技能持续时间 小于0代表一致持续  0代表立即执行销毁  大于0即到时自动销毁</param>
    /// <param name="curLayer">当前层级</param>
    /// <param name="nextIntervalTime">下次间隔触发时间</param>
    /// <returns></returns>
    public SkillEffectBase CreateOneSkillEffect(int skillID, int effectID, long fromID, long targetID, int duration = 0, int curLayer = 1, long nextIntervalTime = 0)
    {
        if (SkillEffectMap == null)
        {
            Log.Error($"createOneSkillEffect Error not init skill effect map");
            return null;
        }

        DRSkillEffect skillEffectCfg = GFEntryCore.DataTable.GetDataTable<DRSkillEffect>().GetDataRow(effectID);
        if (skillEffectCfg == null)
        {
            Log.Error($"createOneSkillEffect Error skillEffectCfg is null  skillID = {skillID} effectID = {effectID}");
            return null;
        }

        if (!SkillEffectMap.ContainsKey((eSkillEffectType)skillEffectCfg.EffectType))
        {
            Log.Error($"createOneSkillEffect Error EffectType is Unknown  skillID = {skillID} effectID = {effectID}");
            return null;
        }
        Type skillEffectClass = SkillEffectMap[(eSkillEffectType)skillEffectCfg.EffectType];
        SkillEffectBase effect = SkillEffectBase.Create(skillEffectClass);
        effect.SetData(skillID, skillEffectCfg, fromID, targetID, duration, curLayer, nextIntervalTime);
        return effect;
    }
}