using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
/// <summary>
/// 通用配置表工具类
/// </summary>
public static class TableUtil
{
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

    /// <summary>
    /// 获取配置表中的语言
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultStr">默认字符串，如果传入了该参数，读取不到目标语言的时候返回该值</param>
    /// <returns></returns>
    public static string GetLanguage(int id, string defaultStr = "")
    {
        DRLanguage drLanguage = GFEntryCore.DataTable.GetDataTable<DRLanguage>().GetDataRow(id);
        if (drLanguage == null)
        {
            Log.Error($"GetLanguage DRLanguage is null id = {id}");
            return string.IsNullOrEmpty(defaultStr) ? $"#{id}" : defaultStr;
        }

        if (string.IsNullOrEmpty(drLanguage.Value))//如果没有翻译为空
        {
            return string.IsNullOrEmpty(defaultStr) ? $"#{id} empty" : defaultStr;
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

    public static bool TryGetGameValue(eGameValueID id, out DRGameValue drGameValue)
    {
        drGameValue = GFEntryCore.DataTable.GetDataTable<DRGameValue>().GetDataRow((int)id);
        return drGameValue != null;
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
        DateTime now = TimeUtil.TimeStamp2DataTime(TimeUtil.GetServerTimeStamp());
        double minutesNow = now.TimeOfDay.TotalMinutes;
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

    /// <summary>
    /// 检测场景进入模式(单人|队伍)
    /// </summary>
    /// <param name="mapId"></param>
    /// <param name="challengeType"></param>
    /// <returns></returns>
    public static bool CheckDungeonChallenge(int mapId, GameMessageCore.SceneChallengeType challengeType)
    {
        DRSceneArea areaRow = GFEntryCore.DataTable.GetDataTable<DRSceneArea>().GetDataRow(mapId);
        if (areaRow == null)
        {
            return false;
        }

        foreach (int mode in areaRow.ChallengeMode)
        {
            if (mode == (int)challengeType)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获取怪物随机潜力值
    /// 返回的是int，代表千分位的浮点数
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int GetMonsterRandomPotentiality(DRMonster cfg, GameMessageCore.TalentType type)
    {
        int[] valueRange = null;
        switch (type)
        {
            case GameMessageCore.TalentType.Gather:
                valueRange = cfg.GatherPotentiality;
                break;
            case GameMessageCore.TalentType.Battle:
                valueRange = cfg.CombatPotentiality;
                break;
            case GameMessageCore.TalentType.Farming:
                valueRange = cfg.FarmPotentiality;
                break;
        }

        if (valueRange == null || valueRange.Length != 2)
        {
            Log.Error($"GetMonsterRandomPotentialValue valueRange error valueRange = {valueRange}");
            return 0;
        }

        int minValue = valueRange[0];
        int maxValue = valueRange[1];
        return UnityEngine.Random.Range(minValue, maxValue);
    }

    /// <summary>
    /// 用配置表中的初始化属性来设置属性组件 这里不会考虑潜力值
    /// </summary>
    /// <param name="attributeCpt"></param>
    /// <param name="attr"></param>
    public static void SetTableInitAttribute(AttributeDataCpt attributeCpt, int[][] attr)
    {
        if (attributeCpt == null)
        {
            Log.Error($"SetTableInitAttribute attributeCpt is null");
            return;
        }

        //遍历属性数组
        ForeachAttribute(attr, (type, baseValue, affectByPotential) =>
        {
            attributeCpt.SetBaseValue(type, baseValue);
            return false;
        });
    }

    /// <summary>
    /// 遍历属性数组
    /// </summary>
    /// <param name="attr">属性二维数组</param>
    /// <param name="cb">T0:属性类型，T1:属性值，T2:属性是否受潜力值影响,返回值：是否停止遍历</param>
    public static void ForeachAttribute(int[][] attr, Func<eAttributeType, int, bool, bool> cb)
    {
        if (attr == null || attr.Length == 0)
        {
            return;
        }

        if (cb == null)
        {
            Log.Error($"ForeachAttribute cb is null");
            return;
        }

        foreach (int[] item in attr)
        {
            if (item.Length != 3)
            {
                Log.Error($"ForeachAttribute item.Length != 3");
                continue;
            }
            eAttributeType type = (eAttributeType)item[0];
            int value = item[1];
            bool affectByPotential = item[2] > 0;
            if (cb.Invoke(type, value, affectByPotential))
            {
                break;
            }
        }
    }

    /// <summary>
    /// 获取一个配置具体项
    /// </summary>
    /// <param name="cid"></param>
    /// <typeparam name="T">哪个表</typeparam>
    /// <returns></returns>
    public static T GetConfig<T>(int cid) where T : IDataRow
    {
        return GFEntryCore.DataTable.GetDataTable<T>().GetDataRow(cid);
    }

    /// <summary>
    /// 获取副本关卡时间限制
    /// </summary>
    public static float GetInstancingLevelTimeLimit(DRSceneArea drSceneArea, int index)
    {
        float timeLimit = 0;
        if (drSceneArea.SceneSubtype == (int)GameMessageCore.DungeonSubType.Pvp && index == 0)
        {
            DRGameValue drGameValue = GetGameValue(eGameValueID.InstancingMatchTime);
            timeLimit = drGameValue.Value;
        }
        else
        {
            if (index >= 0 && index < drSceneArea.ChapterTimeLimit.Length)
            {
                timeLimit = drSceneArea.ChapterTimeLimit[index];
            }
            else
            {
                Log.Error($"GetInstancingTimeLimit Error: ChapterTimeLimit:{index} is out of range");
            }
        }

        return timeLimit;
    }

    /// <summary>
    /// 获取副本关卡事件
    /// </summary>
    public static int[] GetInstancingLevelEventList(DRSceneArea drSceneArea, int index)
    {
        int[] eventList = null;
        if (index >= 0 && index < drSceneArea.ChapterEvents.Length)
        {
            eventList = drSceneArea.ChapterEvents[index];
        }
        return eventList;
    }

    /// <summary>
    /// 获取副本关卡最大评分
    /// </summary>
    public static int GetInstancingLevelMaxScore(DRSceneArea drSceneArea, int index)
    {
        int maxScore = 0;
        if (index >= 0 && index < drSceneArea.ChapterProgress.Length)
        {
            maxScore = drSceneArea.ChapterProgress[index];
        }
        return maxScore;
    }

    /// <summary>
    /// 获取宠物原始属性信息
    /// </summary>
    /// <returns>(属性值，是否受到潜力值影响)</returns>
    public static (int, bool) GetPetRawAttrInfo(int petCid, eAttributeType attrType)
    {
        DRPet cfg = GetConfig<DRPet>(petCid);
        if (cfg == null)
        {
            Log.Error($"GetPetRawAttr: pet config is null, petId={petCid}");
            return (0, false);
        }
        int value = 0;
        bool resAffectByPotential = false;
        ForeachAttribute(cfg.InitialAttribute, (type, val, affectByPotential) =>
        {
            if (type == attrType)
            {
                value = val;
                resAffectByPotential = affectByPotential;
                return true;
            }

            return false;
        });

        return (value, resAffectByPotential);
    }

    /// <summary>
    /// 获取属性值显示
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="displayDecimal"></param>
    /// <returns></returns>
    public static string GetAttrDisplay(eAttributeType type, int value, bool displayDecimal = true)
    {
        DREntityAttribute cfg = GetConfig<DREntityAttribute>((int)type);
        if (cfg == null)
        {
            return "";
        }

        float coefficient = GetAttributeCoefficient(type);
        if (cfg.ValueType == (int)eAttributeValueType.ThousandthPct)
        {
            return (value * coefficient * MathUtilCore.PC).ToString("F2") + "%";
        }
        else if (cfg.ValueType == (int)eAttributeValueType.Thousandth)
        {
            return (value * coefficient).ToString(displayDecimal ? "F2" : "F0");
        }
        else if (cfg.ValueType == (int)eAttributeValueType.Int)
        {
            return value.ToString();
        }

        return value.ToString();
    }
    /// <summary>
    /// 根据配置中的int[][]生成某个属性组件上的属性修改器列表 配置为可同时修改多个属性
    /// </summary>
    /// <param name="attributeCpt">对应实体的属性组件</param>
    /// <param name="parameters">二维int参数 eg:"15,4,200,15,4;13,4,100,13,4"</param>
    /// <param name="excludeType">被排除的修改器类型 某些类型不能直接设置属性组件获取修改器 需要外部自己处理 比如HP 但是给到外面解析出来的值方便外部处理</param>
    /// <param name="excludeValue">被排除的修改类型对应的值</param>
    /// <returns></returns>
    public static List<IntAttributeModifier> GenerateAttributeModify(AttributeDataCpt attributeCpt, int[][] parameters, out eAttributeType excludeType, out int excludeValue)
    {
        List<IntAttributeModifier> list = new();
        excludeType = eAttributeType.Unknown;
        excludeValue = 0;

        for (int index = 0; index < parameters.Length; index++)
        {
            try
            {
                eAttributeType attributeType = (eAttributeType)parameters[index][0];
                eModifierType modifierType = (eModifierType)parameters[index][1];
                int value = parameters[index][2];
                //基于其它属性进行加成计算
                if (parameters[index].Length > 4)
                {
                    eAttributeType addType = (eAttributeType)parameters[index][3]; //加成属性类型
                    eModifierType addModifierType = (eModifierType)parameters[index][4]; //加成类型
                    int addValue;
                    if (addModifierType == eModifierType.PctAdd)
                    {
                        addValue = attributeCpt.GetBaseValue(addType);
                        value = (int)(addValue * value / IntAttribute.PERCENTAGE_FLAG);
                    }
                    else if (addModifierType == eModifierType.FinalPctAdd)
                    {
                        addValue = attributeCpt.GetValue(addType);
                        value = (int)(addValue * value / IntAttribute.PERCENTAGE_FLAG);
                    }
                }

                //血量做特殊处理，这里不添加修改器 给外部处理
                if (attributeType == eAttributeType.HP)
                {
                    excludeType = attributeType;
                    excludeValue = value;
                }
                else
                {
                    IntAttributeModifier modifier = attributeCpt.AddModifier(attributeType, modifierType, value);
                    list.Add(modifier);
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"GenerateAttributeModify error from table index = {index} e = {e}");
                continue;
            }
        }

        return list;
    }

    /// <summary>
    /// 配置表中的枚举偏移量数组转换成一个枚举值
    /// </summary>
    /// <returns></returns>
    public static T ConvertToBitEnum<T>(int[] multiBit) where T : Enum
    {
        if (multiBit == null || multiBit.Length == 0)
        {
            return (T)Enum.ToObject(typeof(T), 0);
        }

        int res = 0;
        foreach (int item in multiBit)
        {
            res |= 1 << item;
        }
        return (T)Enum.ToObject(typeof(T), res);
    }

    /// <summary>
    /// 配置中单个代表枚举偏移量的配置转换成实际枚举值 如果参数为0则返回枚举0比较特殊 代表空
    /// </summary>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static T ConvertToBitEnum<T>(int bit) where T : Enum
    {
        if (bit == 0)
        {
            return (T)Enum.ToObject(typeof(T), 0);
        }

        return (T)Enum.ToObject(typeof(T), 1 << bit);
    }
}