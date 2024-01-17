
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public static partial class SkillUtil
{
    /// <summary>
    /// 点对点命中检测
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="blockLayer">遮挡层</param>
    /// <returns></returns>
    public static bool CheckP2P(Vector3 origin, Vector3 target, int blockLayer)
    {
        if (origin.ApproximatelyEquals(target))
        {
            return true;
        }

        bool result = !Physics.Linecast(origin, target, blockLayer, QueryTriggerInteraction.Ignore);
        return result;
    }

    /// <summary>
    /// 点对点命中检测
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="hitInfo"></param>
    /// <param name="blockLayer">遮挡层</param>
    /// <returns></returns>
    public static bool CheckP2P(Vector3 origin, Vector3 target, out RaycastHit hitInfo, int blockLayer)
    {
        if (origin.ApproximatelyEquals(target))
        {
            hitInfo = default;
            return true;
        }

        bool result = !Physics.Linecast(origin, target, out hitInfo, blockLayer, QueryTriggerInteraction.Ignore);
        return result;
    }

    public static int GetEntityTargetLayer(GameMessageCore.EntityType type, int targetType)
    {
        return (1 << MLayerMask.PLAYER) | (1 << MLayerMask.MONSTER);
    }

    /// <summary>
    /// 搜索目标列表
    /// </summary>
    /// <param name="pos">目标位置</param>
    /// <param name="fromEntity">搜索实体</param>
    /// <param name="skillRange">技能范围</param>
    /// <param name="skillDir">技能方向</param>
    /// <param name="skillTargetType">技能目标类型</param>
    public static List<EntityBase> SearchTargetEntityList(Vector3 pos, EntityBase fromEntity, int[] skillRange, Vector3 skillDir, int skillTargetType = (int)eSkillTargetType.Enemy)
    {
        List<EntityBase> targetEntityList = new();
        int targetLayer = GetEntityTargetLayer(fromEntity.BaseData.Type, skillTargetType);
        Collider[] colliders = SearchTargetColliders(pos, skillRange, skillDir, targetLayer, MLayerMask.MASK_SCENE_OBSTRUCTION);
        if (colliders == null || colliders.Length <= 0)
        {
            return targetEntityList;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out EntityReferenceData refData))
            {
                if (fromEntity.EntityCampDataCore.IsSkillTarget(refData.Entity, skillTargetType))
                {
                    targetEntityList.Add(refData.Entity);
                }
            }
        }
        return targetEntityList;
    }

    /// <summary>
    /// 搜索目标列表
    /// </summary>
    /// <param name="pos">目标位置</param>
    /// <param name="fromEntity">搜索实体</param>
    /// <param name="entityMap">实体Map</param>
    /// <param name="skillRange">技能范围</param>
    /// <param name="skillDir">技能方向</param>
    /// <param name="skillTargetType">技能目标类型</param>
    public static void SearchTargetEntityMap(Vector3 pos, EntityBase fromEntity, HashSet<EntityBase> entityMap, int[] skillRange, Vector3 skillDir, int skillTargetType = (int)eSkillTargetType.Enemy)
    {

        int targetLayer = GetEntityTargetLayer(fromEntity.BaseData.Type, skillTargetType);
        Collider[] colliders = SearchTargetColliders(pos, skillRange, skillDir, targetLayer, MLayerMask.MASK_SCENE_OBSTRUCTION);
        if (colliders == null || colliders.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out EntityReferenceData refData))
            {
                if (fromEntity.EntityCampDataCore.IsSkillTarget(refData.Entity, skillTargetType))
                {
                    if (!entityMap.Contains(refData.Entity))
                    {
                        _ = entityMap.Add(refData.Entity);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 搜索目标碰撞体列表
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillRange"></param>
    /// <param name="skillDir"></param>
    /// <param name="targetLayer">搜索目标层</param>
    /// <param name="blockLayer">阻挡层</param>
    /// <returns></returns>
    public static Collider[] SearchTargetColliders(Vector3 pos, int[] skillRange, Vector3 skillDir, int targetLayer, int blockLayer)
    {
        if (skillRange == null || skillRange.Length <= 0)
        {
            return new Collider[0];
        }

        SkillShapeBase shape = SkillShapeFactory.CreateOneSkillShape(skillRange, pos, skillDir);
        Collider[] colliders = shape.CheckHited(targetLayer, blockLayer);
        SkillShapeBase.Release(shape);
        return colliders;
    }

    /// <summary>
    /// 射线搜索目标列表, 可能为null
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="dir"></param>
    /// <param name="maxDist"></param>
    /// <param name="targetLayer"></param>
    /// <param name="blockLayer"></param>
    /// </summary>
    public static EntityBase RaySearchTargetEntityList(Vector3 startPos, Vector3 dir, float maxDist, int targetLayer, int blockLayer)
    {

        if (Physics.Raycast(startPos, dir, out RaycastHit hit, maxDist, targetLayer | blockLayer))
        {
            if ((targetLayer & (1 << hit.collider.gameObject.layer)) > 0)
            {
                if (hit.collider.gameObject.TryGetComponent(out EntityReferenceData refData))
                {
                    if (refData.Entity != null)
                    {
                        return refData.Entity;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 射线搜索目标列表, 可能为null
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="dir"></param>
    /// <param name="maxDist"></param>
    /// <param name="targetLayer"></param>
    /// <param name="blockLayer"></param>
    /// </summary>
    public static List<EntityBase> RayAllSearchTargetEntityList(Vector3 startPos, Vector3 dir, float maxDist, int targetLayer, int blockLayer)
    {
        RaycastHit[] hitList = Physics.RaycastAll(startPos, dir, maxDist, targetLayer | blockLayer);
        List<EntityBase> entityList = new();
        for (int i = 0; i < hitList.Length; i++)
        {
            if ((targetLayer & (1 << hitList[i].collider.gameObject.layer)) > 0)
            {
                if (hitList[i].collider.gameObject.TryGetComponent(out EntityReferenceData refData))
                {
                    if (refData.Entity != null)
                    {
                        entityList.Add(refData.Entity);
                    }
                }
            }
        }
        return entityList;
    }

    /// <summary>
    // 射线检测碰撞的位置，没有碰撞则返回射线最远终点位置
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="dir"></param>
    /// <param name="maxDist"></param>
    /// <param name="targetLayer"></param>
    /// <param name="blockLayer"></param>
    /// </summary>
    public static Vector3 RaySearchPos(Vector3 startPos, Vector3 dir, float maxDist, int targetLayer, int blockLayer)
    {

        if (Physics.Raycast(startPos, dir, out RaycastHit hit, maxDist, targetLayer | blockLayer))
        {
            if ((targetLayer & (1 << hit.collider.gameObject.layer)) > 0)
            {
                return hit.point;
            }
        }
        return startPos + dir * maxDist;
    }
    /// <summary>
    /// 计算技能CD
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static long CalculateSkillCD(int skillID, EntityBase entity)
    {
        DRSkill CurSkillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (CurSkillCfg == null)
        {
            Log.Error($"CalculateSkillCD  Error skillID = {skillID}");
            return 0;
        }

        long skillCD = CurSkillCfg.SkillCD;
        if (skillCD < 0)
        {
            skillCD = 0;
        }
        return skillCD;
    }

    /// <summary>
    /// 实体技能效果执行
    /// </summary>
    /// <param name="inputData">输入数据</param>
    /// <param name="effectList">效果列表</param>
    /// <param name="fromEntity">释放实体</param>
    /// <param name="targetEntity">目标实体</param>
    /// <returns></returns>
    public static List<GameMessageCore.DamageEffect> EntitySkillEffectExecute(InputSkillReleaseData inputData, int[] effectList, EntityBase fromEntity, EntityBase targetEntity)
    {
        List<GameMessageCore.DamageEffect> effects = new();
        if (effectList == null || effectList.Length <= 0)
        {
            return effects;
        }
        SkillEffectCpt effectCpt = targetEntity.GetComponent<SkillEffectCpt>();
        List<SkillEffectBase> skillEffects = SkillConfigParse.ParseSkillEffect(inputData.SkillID, fromEntity.BaseData.Id, targetEntity.BaseData.Id, effectList);
        for (int i = 0; i < skillEffects.Count; i++)
        {
            try
            {
                SkillEffectBase skillEffect = skillEffects[i];
                if (effectCpt.CheckApplyEffect(fromEntity, targetEntity, skillEffect))
                {
                    GameMessageCore.DamageEffect effectData = skillEffect.CreateEffectData(fromEntity, targetEntity, inputData);
                    if (effectData == null)
                    {
                        continue;
                    }
                    effectData.EffectType = (GameMessageCore.DamageEffectId)skillEffect.EffectCfg.Id;
                    skillEffect.SetEffectData(effectData);
                    skillEffect.SetInputData(inputData);
                    effects.Add(effectData);
                    if (!inputData.IsPreRelease)
                    {
                        fromEntity.EntityEvent.BeforeGiveSkillEffect?.Invoke(targetEntity, effectData);
                        targetEntity.EntityEvent.BeforeApplySkillEffect?.Invoke(effectData);
                        effectCpt.ApplyOneEffect(skillEffect);//注意顺序，Effects如果是瞬间的，应用后会立即被清除
                        fromEntity.EntityEvent.AfterGiveSkillEffect?.Invoke(targetEntity, effectData);
                        targetEntity.EntityEvent.AfterApplySkillEffect?.Invoke(effectData);
                    }
                    else
                    {
                        skillEffect.PlayPreEffect(targetEntity);
                        skillEffect.Dispose();
                    }

                }
                else
                {
                    skillEffect.Dispose();
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"skill cast skill effect apply error skillID = {inputData.SkillID}, effectID = {skillEffects[i].EffectID} error = {e}");
                continue;
            }
        }
        return effects;
    }
    /// <summary>
    /// 实体技能效果执行
    /// </summary>
    /// <param name="inputData">输入数据</param>
    /// <param name="effectList">效果列表</param>
    /// <param name="fromEntity">释放实体</param>
    /// <param name="targetEntity">目标实体</param>
    /// <returns></returns>
    public static List<GameMessageCore.DamageEffect> EntitySkillEffectExecuteMiss(InputSkillReleaseData inputData, EntityBase fromEntity, EntityBase targetEntity)
    {
        List<GameMessageCore.DamageEffect> effects = new();

        try
        {
            GameMessageCore.DamageEffect effectData = SkillDamage.CreateSpecialDamageEffect(GameMessageCore.DamageState.Miss, targetEntity.BattleDataCore.HP, 0);
            effects.Add(effectData);
            if (inputData.IsPreRelease)
            {
                SkillEffectBase skillEffect = GFEntryCore.SkillEffectFactory.CreateOneSkillEffect(inputData.SkillID, TableDefine.DAMAGE_EFFECT_ID, fromEntity.BaseData.Id, targetEntity.BaseData.Id, 0);
                skillEffect.SetEffectData(effectData);
                skillEffect.SetInputData(inputData);
                skillEffect.PlayPreEffect(targetEntity);
                skillEffect.Dispose();
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"EntitySkillEffectExecuteMiss Error  = {inputData.SkillID}, error = {e}");
        }

        return effects;
    }
    /// <summary>
    /// 实体技能效果取消
    /// </summary>
    /// <param name="skillID">技能ID</param>
    /// <param name="effectList">效果列表</param>
    /// <param name="fromEntity">释放实体</param>
    /// <param name="targetEntity">目标实体</param>
    /// <returns></returns>
    public static void EntityAbolishSkillEffect(int skillID, int[] effectList, EntityBase fromEntity, EntityBase targetEntity)
    {
        SkillEffectCpt effectCpt = targetEntity.GetComponent<SkillEffectCpt>();
        if (effectList != null && effectList.Length > 0)
        {
            for (int i = 0; i < effectList.Length; i++)
            {
                effectCpt.AbolishSkillEffect(effectList[i], fromEntity.BaseData.Id);
            }
        }
    }
    /// <summary>
    /// 计算技能效果列表
    /// </summary>
    /// <param name="modifierList"></param>
    /// <returns></returns>
    public static List<int> CalculateSkillEffectModifierList(List<SkillEffectModifier> modifierList)
    {
        Dictionary<int, int> effectValueMap = new();
        bool isReplace = false;
        for (int i = 0; i < modifierList.Count; i++)
        {
            SkillEffectModifier modifier = modifierList[i];
            if (modifier.Type == eSkillEffectModifierType.Replace)
            {
                effectValueMap.Clear();
                isReplace = true;
            }

            for (int j = 0; j < modifier.EffectIDs.Length; j++)
            {
                int effectID = modifier.EffectIDs[j];
                if (effectValueMap.TryGetValue(effectID, out int count))
                {
                    effectValueMap[effectID] = count + modifier.Value;
                }
                else
                {
                    effectValueMap.Add(effectID, modifier.Value);
                }
            }

            if (isReplace)
            {
                break;
            }
        }

        List<int> effectList = new();
        foreach (KeyValuePair<int, int> item in effectValueMap)
        {
            if (item.Value > 0)
            {
                effectList.Add(item.Key);
            }
        }
        return effectList;
    }

    public static int[] GetSkillEffect(EntityBase entity, DRSkill drSkill, eSkillEffectApplyType type)
    {
        //拥有技能, 效果可能是动态的，所以需要从技能组件中获取
        SkillBase skill = entity.GetComponent<SkillCpt>().GetSkill(drSkill.Id);
        if (skill != null)
        {
            return skill.GetEffect(type);
        }

        //没有技能， 效果是静态的，直接从配置中获取
        return type switch
        {
            eSkillEffectApplyType.Init => drSkill.EffectInit,
            eSkillEffectApplyType.Forward => drSkill.EffectForward,
            eSkillEffectApplyType.CastSelf => drSkill.EffectSelf,
            eSkillEffectApplyType.CastEnemy => drSkill.EffectEnemy,
            _ => null,
        };
    }

    public static bool IsSceneDeath(GameMessageCore.DamageState dmgState)
    {
        return dmgState is GameMessageCore.DamageState.Fall or GameMessageCore.DamageState.WaterDrown;
    }

    /// <summary>
    /// 计算飞行物花费时间
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="skillCfg"></param>
    /// <returns>(花费时间，最终位置)</returns>
    public static (float, Vector3) CalcFlyCostTime(Vector3 startPos, Vector3 endPos, DRSkillFlyer skillFlyerCfg)
    {
        //固定时间
        if (skillFlyerCfg.FlyTime > 0)
        {
            return (skillFlyerCfg.FlyTime * TimeUtil.MS2S, endPos);
        }

        //固定速度
        float flySpeed = 1;
        if (skillFlyerCfg.FlySpeed > 0)
        {
            flySpeed = skillFlyerCfg.FlySpeed * MathUtilCore.CM2M;
        }
        else
        {
            Log.Error($"技能:{skillFlyerCfg.Id} 发射飞行物时，配置的飞行速度{skillFlyerCfg.FlySpeed}不合法");
        }
        float distance = Vector3.Distance(startPos, endPos);
        float maxDist = skillFlyerCfg.FlyDistance * MathUtilCore.CM2M;
        Vector3 finalPos = endPos;
        if (distance > maxDist)
        {
            distance = maxDist;
            finalPos = startPos + (endPos - startPos).normalized * maxDist;
        }
        float costTime = distance / flySpeed;
        return (costTime, finalPos);
    }

    public static int GetFlag(int[] flags)
    {
        int flag = 0;
        if (flags != null)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                flag |= 1 << flags[i];
            }
        }
        return flag;
    }
}