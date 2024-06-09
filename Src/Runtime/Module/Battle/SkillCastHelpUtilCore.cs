/*
 * @Author: xiang huan
 * @Date: 2023-06-25 10:26:23
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/SkillCastHelpUtilCore.cs
 * 
 */
using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 技能释放帮助工具 
/// </summary>
public static class SkillCastHelpUtilCore
{
    /// <summary>
    /// 计算应用产生一个目标效果 忽略位置 可能返回Null
    /// </summary>
    /// <param name="inputData">技能输入</param>
    /// <param name="fromEntity">来源实体</param>
    /// <param name="targetEntity"></param>
    /// <param name="effectList">效果id组</param>
    /// <returns></returns>
    public static EntityDamage ApplyTargetEffect(InputSkillReleaseData inputData, EntityBase fromEntity, EntityBase targetEntity, int[] effectList)
    {

        bool needHitCheck = inputData.DRSkill.IsCheckHit && fromEntity != targetEntity;//非自己需要检查命中系统
        bool isHit = !needHitCheck || SkillDamage.CheckHit(fromEntity.BattleDataCore, targetEntity.BattleDataCore, inputData.InputRandom);
        List<DamageEffect> effects;
        if (isHit)
        {
            effects = SkillUtil.EntitySkillEffectExecute(inputData, effectList, fromEntity, targetEntity);
        }
        else
        {
            effects = SkillUtil.EntitySkillEffectExecuteMiss(inputData, fromEntity, targetEntity);
        }

        if (effects != null && effects.Count > 0)
        {
            return NetUtilCore.AssembleEntityDamageNetMsg(targetEntity, inputData.SkillID, effects);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 计算应用产生一个范围效果
    /// </summary>
    /// <param name="inputData">技能输入</param>
    /// <param name="fromEntity">来自实体 不会拿该实体位置方向计算 只是采集信息</param>
    /// <param name="effectEnemy">效果id组</param>
    /// <param name="rangePos">范围位置</param>
    /// <returns></returns>
    public static List<EntityDamage> ApplyRangeEffect(InputSkillReleaseData inputData, EntityBase fromEntity, int[] effectIds, UnityEngine.Vector3 rangePos)
    {
        List<EntityDamage> entityDamages = new();

        if (inputData.DRSkill.SkillRange == null || inputData.DRSkill.SkillRange.Length == 0)
        {
            return entityDamages;
        }

        List<EntityBase> targetEntities = SkillUtil.SearchTargetEntityList(rangePos, fromEntity, inputData.DRSkill.SkillRange, inputData.Dir, inputData.TargetType);
        if (targetEntities == null || targetEntities.Count == 0)
        {
            return entityDamages;
        }

        for (int i = 0; i < targetEntities.Count; i++)
        {
            EntityDamage damage = ApplyTargetEffect(inputData, fromEntity, targetEntities[i], effectIds);
            if (damage != null)
            {
                entityDamages.Add(damage);
            }
        }

        return entityDamages;
    }

    /// <summary>
    /// 在一个范围内随机数量的单位触发
    /// </summary>
    /// <param name="inputData">技能输入</param>
    /// <param name="fromEntity">来自实体 不会拿该实体位置方向计算 只是采集信息</param>
    /// <param name="effectEnemy">效果id组</param>
    /// <param name="maxNum">触发单位上限</param>
    /// <param name="rangePos">范围位置</param>
    /// <returns></returns>
    public static List<EntityDamage> ApplyRangeRandomTriggerEffect(InputSkillReleaseData inputData, EntityBase fromEntity, int[] effectIds, int maxNum, UnityEngine.Vector3 rangePos)
    {
        List<EntityDamage> entityDamages = new();

        if (inputData.DRSkill.SkillRange == null || inputData.DRSkill.SkillRange.Length == 0)
        {
            Log.Error($"技能范围配置错误 skillID:{inputData.SkillID}");
            return entityDamages;
        }

        List<EntityBase> targetEntities = SkillUtil.SearchTargetEntityList(rangePos, fromEntity, inputData.DRSkill.SkillRange, inputData.Dir, inputData.TargetType);
        if (targetEntities == null || targetEntities.Count == 0)
        {
            return entityDamages;
        }

        if (targetEntities.Count > maxNum)
        {
            targetEntities = MathUtilCore.RandomSortList(targetEntities);
        }


        for (int i = 0; i < targetEntities.Count && i < maxNum; i++)
        {
            EntityDamage damage = ApplyTargetEffect(inputData, fromEntity, targetEntities[i], effectIds);
            if (damage != null)
            {
                entityDamages.Add(damage);
            }
        }

        return entityDamages;
    }

    public static List<EntityDamage> ApplyTargetListEffect(EntityBase castEntity, InputSkillReleaseData inputData, int[] effectList)
    {
        List<EntityDamage> entityDamages = new();
        for (int i = 0; i < inputData.Targets.Length; i++)
        {
            EntityBase targetEntity = GFEntryCore.GetModule<IEntityMgr>().GetEntity<EntityBase>(inputData.Targets[i]);
            if (targetEntity == null)
            {
                continue;
            }
            EntityDamage damage = ApplyTargetEffect(inputData, castEntity, targetEntity, effectList);
            if (damage != null)
            {
                entityDamages.Add(damage);
            }
        }
        return entityDamages;
    }
    public static List<EntityDamage> ApplyTargetListRangeEffect(EntityBase castEntity, InputSkillReleaseData inputData, int[] effectList)
    {
        List<EntityDamage> entityDamages = new();
        if (inputData.TargetPosList == null || inputData.TargetPosList.Length == 0)
        {
            Log.Error($"ApplyTargetListRangeEffect TargetPosList is null:{inputData.SkillID}");
            return entityDamages;
        }

        for (int i = 0; i < inputData.TargetPosList.Length; i++)
        {
            List<EntityDamage> damages = ApplyRangeEffect(inputData, castEntity, effectList, inputData.TargetPosList[i]);
            if (damages != null && damages.Count > 0)
            {
                entityDamages.AddRange(damages);
            }
        }
        return entityDamages;
    }
    /// <summary>
    /// 输入数据
    /// </summary>
    /// <param name="castEntity">释放实体</param>
    /// <param name="castPos">释放位置</param>
    /// <param name="inputData">输入数据</param>
    /// <returns></returns>
    public static List<EntityDamage> SkillCastExecute(EntityBase castEntity, UnityEngine.Vector3 castPos, InputSkillReleaseData inputData)
    {
        List<EntityDamage> damages = new();
        if (inputData.DRSkill == null)
        {
            Log.Error($"SkillCastExecute error skillCfg is Null ID = {inputData.SkillID}");
            return damages;
        }
        // 技能独立逻辑（一些技能配置在skill表，但是因为一些skill功能非常独立，与一般技能非常不通用，不在这里处理。独立技能在自己的模块里监听处理即可）
        if (inputData.DRSkill.IsIndependentLogic)
        {
            return damages;
        }

        //应用对自己的效果
        EntityDamage selfDamage = ApplyTargetEffect(inputData, castEntity, castEntity, SkillUtil.GetSkillEffect(castEntity, inputData.DRSkill, eSkillEffectApplyType.CastSelf));
        if (selfDamage != null)
        {
            damages.Add(selfDamage);
        }


        int targetFlag = SkillUtil.GetFlag(inputData.DRSkill.TargetFlag);
        List<EntityDamage> enemyDamages;
        if ((targetFlag & (int)eSkillTargetFlag.Target) != 0 && inputData.Targets != null && inputData.Targets.Length > 0)  //有目标
        {
            enemyDamages = ApplyTargetListEffect(castEntity, inputData, SkillUtil.GetSkillEffect(castEntity, inputData.DRSkill, eSkillEffectApplyType.CastEnemy));
        }
        else if ((targetFlag & (int)eSkillTargetFlag.Pos) != 0 && inputData.TargetPosList != null && inputData.TargetPosList.Length > 0) //有目标位置
        {
            enemyDamages = ApplyTargetListRangeEffect(castEntity, inputData, SkillUtil.GetSkillEffect(castEntity, inputData.DRSkill, eSkillEffectApplyType.CastEnemy));
        }
        else //无目标
        {
            enemyDamages = ApplyRangeEffect(inputData, castEntity, SkillUtil.GetSkillEffect(castEntity, inputData.DRSkill, eSkillEffectApplyType.CastEnemy), castPos);
        }
        damages.AddRange(enemyDamages);
        return damages;
    }
}