using System;
using System.Linq;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
/// <summary>
/// 通用配置表工具类
/// </summary>
public static class TableUtil
{
    /// <summary>
    /// 配置中的家园动作数组转实际枚举 配置中的是左移位数
    /// </summary>
    /// <param name="drAction"></param>
    /// <returns></returns>
    public static HomeDefine.eAction ToHomeAction(int[] drAction)
    {
        if (drAction == null || drAction.Length == 0)
        {
            return HomeDefine.eAction.None;
        }

        HomeDefine.eAction action = HomeDefine.eAction.None;
        foreach (int item in drAction)
        {
            action |= ToHomeAction(item);
        }
        return action;
    }

    /// <summary>
    /// 配置中的家园动作转实际枚举 配置中的是左移位数
    /// </summary>
    /// <param name="drAction"></param>
    /// <returns></returns>
    public static HomeDefine.eAction ToHomeAction(int drAction)
    {
        if (drAction == 0)
        {
            return HomeDefine.eAction.None;
        }

        return (HomeDefine.eAction)(1 << drAction);
    }

    /// <summary>
    /// 配置表中的字符串格式化输入 xxx{0}bbbb{1}
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string StringFormat(string format, params object[] args)
    {
        try
        {
            string res = string.Format(format, args);
            return res;
        }
        catch (System.Exception e)
        {
            Log.Error($"table StringFormat error format = {format} args = {args} e = {e}");
            return format;
        }
    }

    public static string GetLanguage(int id)
    {
        DRLanguage drLanguage = GFEntryCore.DataTable.GetDataTable<DRLanguage>().GetDataRow(id);
        if (drLanguage == null)
        {
            Log.Error($"GetLanguage DRLanguage is null id = {id}");
            return $"#{id}";
        }
        return drLanguage.Value;
    }

    public static DRGameValue GetGameValue(eGameValueID id)
    {
        DRGameValue drGameValue = GFEntryCore.DataTable.GetDataTable<DRGameValue>().GetDataRow((int)id);
        if (drGameValue == null)
        {
            Log.Error($"GetGameValue DRGameValue is null id = {id}");
            throw new System.Exception($"GetGameValue not find id = {id}");
        }
        return drGameValue;
    }

    /// <summary>
    /// 获取天赋节点收益，可能返回null
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="lv"></param>
    /// <param name="gainsType"></param>
    /// <returns></returns>
    public static List<int> GetTalentNodeGains(DRTalentTree cfg, int lv, int gainsType)
    {
        if (cfg == null)
        {
            return null;
        }

        if (lv < 1 && lv > cfg.LvLimit)
        {
            Log.Error($"GetTalentNodeGains lv error lv = {lv} cfg.LvLimit = {cfg.LvLimit}");
            return null;//0级不会有收益
        }

        string[] gainArgsArr = cfg.GainsArgs.Split(";");
        if (lv - 1 >= gainArgsArr.Length)
        {
            return null;
        }

        string[] targetLvGainArgArr = gainArgsArr[lv - 1].Split(",");
        //获取gainsType在cfg.GainsType中的索引
        int gainsTypeIndex = Array.IndexOf(cfg.GainsType, gainsType);
        if (gainsTypeIndex < 0 || gainsTypeIndex >= targetLvGainArgArr.Length)
        {
            return null;
        }

        List<int> gainsList = new();
        string[] gainsArr = targetLvGainArgArr[gainsTypeIndex].Split("#");
        foreach (string item in gainsArr)
        {
            if (int.TryParse(item, out int value))
            {
                gainsList.Add(value);
            }
        }

        return gainsList;
    }

    /// <summary>
    /// 获取天赋节点技能收益
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static List<int> GetTalentNodeSkillGains(DRTalentTree cfg, int lv)
    {
        List<int> activeSkillList = GetTalentNodeGains(cfg, lv, (int)GameMessageCore.TalentGainsType.ActiveSkill);
        List<int> passiveSkillList = GetTalentNodeGains(cfg, lv, (int)GameMessageCore.TalentGainsType.PassiveSkill);

        List<int> skillList = null;
        if (activeSkillList != null && activeSkillList.Count > 0)
        {
            skillList = activeSkillList;
        }

        if (passiveSkillList != null && passiveSkillList.Count > 0)
        {
            skillList ??= new List<int>();
            skillList.AddRange(passiveSkillList);
        }

        return skillList;
    }

    /// <summary>
    /// 获取某个eAttributeType的单位真实值系数
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static float GetAttributeCoefficient(eAttributeType type)
    {
        float coefficient = 1f;
        DREntityAttribute dREntityAttribute = GFEntryCore.DataTable.GetDataTable<DREntityAttribute>().GetDataRow((int)type);
        if (dREntityAttribute != null)
        {
            if (dREntityAttribute.ValueType is ((int)eAttributeValueType.Thousandth) or ((int)eAttributeValueType.ThousandthPct))
            {
                coefficient = 1 / 1000f;
            }
        }
        return coefficient;
    }

    /// <summary>
    /// Attribute 中配置表单位值转真实单位值
    /// </summary>
    /// <param name="tableValue"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static float AttributeTableValueConvertToReal(int tableValue, eAttributeType type)
    {
        float coefficient = GetAttributeCoefficient(type);
        return tableValue * coefficient;
    }

    /// <summary>
    /// Attribute 中真实单位值转配置表单位值
    /// </summary>
    /// <param name="realValue"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int AttributeRealValueConvertToTable(float realValue, eAttributeType type)
    {
        float coefficient = GetAttributeCoefficient(type);
        return (int)(realValue / coefficient);
    }

    /// <summary>
    /// 检查当前时间(hour)场景是否开启
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public static bool SceneIsOpened(int mapId)
    {
        DRSceneArea areaRow = GFEntryCore.DataTable.GetDataTable<DRSceneArea>().GetDataRow(mapId);
        if (areaRow == null)
        {
            Log.Error($"map {mapId} not found");
            return false;
        }
        // 非副本随时可以开启 or 没有配置开启时间段,默认随时可以开启
        if (areaRow.SceneType != (int)GameMessageCore.SceneServiceSubType.Dungeon || areaRow.ReleaseTime.Length == 0)
        {
            return true;
        }

        // 检查开启时间段
        bool opened = false;
        DateTime now = TimeUtil.GetServerTime();
        int minutesNow = (now.Hour * TimeUtil.MinutesOfHour) + now.Minute;
        foreach (int[] timeInfoRow in areaRow.ReleaseTime)
        {
            if (timeInfoRow.Length != 2)
            {
                continue;
            }

            int minutesBegin = timeInfoRow[0];
            int minutesEnd = timeInfoRow[1];

            bool isInPeriod = minutesNow >= minutesBegin && minutesNow < minutesEnd;

            if (isInPeriod)
            {
                opened = true;
                break;
            }
        }
        return opened;
    }
}