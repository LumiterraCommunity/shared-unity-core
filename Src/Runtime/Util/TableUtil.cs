using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
using UnityEngine;
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
    /// 获取配置表中的int值 读不到配置返回参数中的默认值
    /// </summary>
    public static int GetGameValueInt(eGameValueID id, int defaultValue = 0)
    {
        if (TryGetGameValue(id, out DRGameValue drGameValue))
        {
            return drGameValue.Value;
        }

        Log.Error($"GetGameValueInt not find id = {id}");
        return defaultValue;
    }

    /// <summary>
    /// 获取配置表中的千分位转换出来的小数 读不到配置返回参数中的默认值
    /// </summary>
    public static float GetGameValueFromThousands(eGameValueID id, float defaultValue = 0)
    {
        if (TryGetGameValue(id, out DRGameValue drGameValue))
        {
            return drGameValue.Value * TableDefine.THOUSANDTH_2_FLOAT;
        }

        Log.Error($"GetGameValueFromThousands not find id = {id}");
        return defaultValue;
    }

    /// <summary>
    /// 获取配置表中的字符串值 读不到配置返回参数中的默认值
    /// </summary>
    public static string GetGameValueString(eGameValueID id, string defaultValue = "")
    {
        if (TryGetGameValue(id, out DRGameValue drGameValue))
        {
            return drGameValue.StrValue;
        }

        Log.Error($"GetGameValueString not find id = {id}");
        return defaultValue;
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
        return Mathf.RoundToInt(realValue / coefficient);
    }

    /// <summary>
    /// 检查当前时间(hour)场景是否开启 针对mapId的
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

        return IsInOpenTimes(areaRow.ReleaseTime);
    }

    /// <summary>
    /// 检查是否在配置中的开放时间内 更加通用 可以给时区 默认UTC
    /// </summary>
    /// <param name="openTimes"></param>
    /// <returns></returns>
    public static bool IsInOpenTimes(int[][] openTimes, int timeZone = 0)
    {
        //没配置就是全天开放
        if (openTimes == null || openTimes.Length == 0)
        {
            return true;
        }

        // 检查开启时间段
        bool opened = false;
        DateTime now = TimeUtil.TimeStamp2DataTime(TimeUtil.GetServerTimeZoneTimeStamp(timeZone));
        double minutesNow = now.TimeOfDay.TotalMinutes;
        foreach (int[] timeInfoRow in openTimes)
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

        return GetRandomPotentiality(valueRange);
    }

    /// <summary>
    /// 获取随机潜力值
    /// 返回的是int，代表千分位的浮点数
    /// </summary>
    /// <param name="valueRange"></param>
    /// <returns></returns>
    public static int GetRandomPotentiality(int[] valueRange)
    {
        if (valueRange == null || valueRange.Length != 2)
        {
            Log.Error($"GetRandomPotentiality valueRange error valueRange = {valueRange}");
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
    /// 用配置表中的初始化属性来设置属性组件 需要提供lv和潜力值计算出来
    /// </summary>
    /// <param name="attributeCpt"></param>
    /// <param name="attr"></param>
    /// <param name="lv"></param>
    /// <param name="potentiality"></param>
    public static void SetTableInitAttribute(AttributeDataCpt attributeCpt, int[][] attr, float lv, float potentiality)
    {
        if (attributeCpt == null)
        {
            Log.Error($"SetTableInitAttribute attributeCpt is null");
            return;
        }

        //遍历属性数组
        ForeachAttribute(attr, (type, baseValue, affectByPotential) =>
        {
            int finalValue;
            if (affectByPotential)//判断是否受潜力值影响
            {
                finalValue = AttributeUtilCore.GetValueByPotentiality(baseValue, potentiality, lv);
            }
            else
            {
                finalValue = baseValue;
            }

            attributeCpt.SetBaseValue(type, finalValue);
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
            if (!ParseAttribute(item, out eAttributeType type, out int value, out bool affectByPotential))
            {
                continue;
            }

            try
            {
                if (cb.Invoke(type, value, affectByPotential))
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Log.Error($"ForeachAttribute invoke error e = {e}");
                continue;
            }
        }
    }

    /// <summary>
    /// 解析属性数组 eg: [1,100,1] 返回是否解析成功
    /// </summary>
    /// <param name="att"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="affectByPotential">是否受潜力值影响</param>
    public static bool ParseAttribute(int[] att, out eAttributeType type, out int value, out bool affectByPotential)
    {
        type = eAttributeType.Unknown;
        value = 0;
        affectByPotential = false;

        if (att.Length != 3)
        {
            Log.Error($"TryParseAttribute error item.Length != 3");
            return false;
        }

        type = (eAttributeType)att[0];
        value = att[1];
        affectByPotential = att[2] > 0;
        return true;
    }

    /// <summary>
    /// 获取一个配置具体项
    /// </summary>
    /// <param name="cid"></param>
    /// <typeparam name="T">哪个表</typeparam>
    /// <returns></returns>
    public static T GetConfig<T>(int cid) where T : class, IDataRow
    {
        return GFEntryCore.DataTable.GetDataTable<T>().GetDataRow(cid);
    }

    /// <summary>
    /// 尝试获取一个配置具体项
    /// </summary>
    /// <typeparam name="T">哪个表</typeparam>
    /// <param name="cid"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    public static bool TryGetConfig<T>(int cid, out T dr) where T : class, IDataRow
    {
        try
        {
            dr = GetConfig<T>(cid);
            return dr != null;
        }
        catch (Exception e)
        {
            Log.Error($"TryGetConfig exception,type:{typeof(T).Name},cid:{cid} e = {e}");
            dr = null;
            return false;
        }
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
    /// 获取副本章节表数据
    /// </summary>
    public static DRSceneAreaChapter GetInstancingChapter(DRSceneArea drSceneArea, int index)
    {
        if (index < 0 || index >= drSceneArea.ChapterIds.Length)
        {
            Log.Error($"GetInstancingChapter Error: ChapterIds:{index} is out of range");
            return null;
        }

        int chapterId = drSceneArea.ChapterIds[index];
        DRSceneAreaChapter drSceneAreaChapter = GetConfig<DRSceneAreaChapter>(chapterId);
        if (drSceneAreaChapter == null)
        {
            Log.Error($"GetInstancingChapter Error: drSceneAreaChapter is null, chapterId:{chapterId}");
        }
        return drSceneAreaChapter;
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
            return (value * coefficient / MathUtilCore.PC).ToString("F2") + "%";
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
    /// <param name="battleData">对应实体的战斗数据组件 可以为空</param>
    /// <param name="parameters">二维int参数 eg:"15,4,200,15,4;13,4,100,13,4"</param>
    /// <returns></returns>
    public static List<IntAttributeModifier> GenerateAttributeModify(AttributeDataCpt attributeCpt, EntityBattleDataCore battleData, int[][] parameters)
    {
        List<IntAttributeModifier> list = new();

        int oldHpMax = battleData?.HPMAX ?? 0;
        int oldWhiteHpMax = battleData?.WhiteHPMAX ?? 0;

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

                //血量修改
                if (attributeType == eAttributeType.HP)
                {
                    if (battleData != null)
                    {
                        battleData.ChangeHP(value);
                    }
                }
                else if (attributeType == eAttributeType.WhiteHP)
                {
                    //白血量增加，同时修改最大白血量
                    IntAttributeModifier modifier = attributeCpt.AddModifier(eAttributeType.MaxWhiteHP, eModifierType.Add, value);
                    list.Add(modifier);
                    if (battleData != null)
                    {
                        battleData.SetWhiteHP(battleData.WhiteHP + value);
                    }
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

        battleData?.HpAutoAdaptMax(oldHpMax, oldWhiteHpMax);

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

    /// <summary>
    /// 检查当前强化等级是否有惩罚
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static bool CheckHasPunishmentOfEnhanceLv(int lv)
    {
        int remainder = lv % 10;
        return remainder is not (0 or 3 or 6);
    }

    /// <summary>
    /// 强化等级转强化阶段
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int EnhanceLv2EnhanceStage(int lv)
    {
        if (lv < 0)
        {
            Log.Error($"Invalid enhance lv:{lv}");
            return 0;
        }

        return lv / TableDefine.EQUIPMENT_ENHANCE_STAGE_BASE;
    }

    /// <summary>
    /// 随机资源点数据
    /// </summary>
    /// 
    /// <returns></returns>
    public static int[] RandomPointData(int[][] pointList, Dictionary<int, int> cidNumMap)
    {
        //point: [weight, cid, maxNum, minLv, maxLv]
        List<int> weightList = new();
        List<int[]> points = new();
        for (int i = 0; i < pointList.Length; i++)
        {
            int[] point = pointList[i];
            if (!cidNumMap.TryGetValue(point[1], out int curNum))
            {
                curNum = 0;
            }

            if (curNum < point[2])
            {
                weightList.Add(point[0]);
                points.Add(point);
            }
        }
        if (weightList.Count <= 0)
        {
            return null;
        }

        int index = MathUtilCore.RandomWeightListIndex(weightList);
        int[] pointData = points[index];
        //随机到空实体
        if (pointData[1] == TableDefine.ENTITY_CID_NULL)
        {
            return null;
        }
        return pointData;
    }

    /// <summary>
    /// 检查场景配置是否是有该功能模块
    /// </summary>
    /// <param name="dRSceneArea"></param>
    /// <param name="homeType"></param>
    /// <returns></returns>
    public static bool CheckIsModuleScene(DRSceneArea dRSceneArea, eSceneFunctionModuleType moduleType)
    {
        if (dRSceneArea == null)
        {
            Log.Error(" CheckIsHomeModuleScene dRSceneArea is null.");
            return false;
        }
        eSceneFunctionModuleType modules = ConvertToBitEnum<eSceneFunctionModuleType>(dRSceneArea.FunctionModule);
        return (modules & moduleType) != 0;
    }

    /// <summary>
    /// 获取某个装备属性强化lv级别的增益
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="type"></param>
    /// <param name="enhanceLv"></param>
    /// <returns></returns>
    public static int GetEquipmentAttrEnhanceGain(DREquipment cfg, eAttributeType type, int enhanceLv = 1)
    {
        try
        {
            enhanceLv = Mathf.Clamp(enhanceLv, 1, cfg.MaxEnhancementLevel);
            for (int i = 0; i < cfg.EnhancementAttribute.Length; i++)
            {
                int[] attr = cfg.EnhancementAttribute[i];
                if (attr[0] == (int)type)
                {
                    return attr[1] * enhanceLv;

                }
            }

            return 0;
        }
        catch (Exception e)
        {
            Log.Error($"GetEquipmentAttrEnhanceGain error cid = {cfg.Id} type = {type} enhanceLv = {enhanceLv} e = {e}");
            return 0;
        }
    }

    /// <summary>
    /// 某个声望值下的某个道具是否可以无限制上链
    /// </summary>
    /// <param name="reputation"></param>
    /// <param name="cid"></param>
    /// <returns></returns>
    public static bool IsOnChainNoLimitItem(int reputation, int cid)
    {
        try
        {
            DRGameValue cfg = GetGameValue(eGameValueID.ItemOnChainWhiteList);
            foreach (int[] info in cfg.ValueArray2)
            {
                int minReputationRequire = info[0];
                if (reputation < minReputationRequire)
                {
                    continue;//声望不够，跳过当前配置
                }

                for (int i = 1; i < info.Length; i++)
                {
                    if (info[i] == cid)
                    {
                        return true;//声望足够，且道具在白名单中
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"IsOnChainNoLimitItem error e = {e}");
        }

        return false;//没有检索到配置，或者声望不够，或者道具不在白名单中
    }
}