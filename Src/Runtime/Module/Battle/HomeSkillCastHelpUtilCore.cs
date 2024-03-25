using static HomeDefine;

/// <summary>
/// 家园技能释放帮助工具
/// </summary>
public static class HomeSkillCastHelpUtilCore
{
    /// <summary>
    /// 判断某个技能的某个动作是否需要计算分段伤害 真正计算伤害的地方
    /// 不能单从动作判断 因为有些动作虽然是hold进度动作 但是有些技能是分段伤害的 比如群体浇水
    /// </summary>
    /// <param name="action"></param>
    /// <param name="drSkill"></param>
    /// <returns></returns>
    public static bool ProgressActionIsSegmentDamage(eAction action, DRSkill drSkill)
    {
        if ((action & PROGRESS_ACTION_MASK) == 0)//进度动作都不是
        {
            return false;
        }

        if ((action & HOLD_PROGRESS_ACTION_MASK) != 0 && drSkill.IsHoldSkill)//浇水动作有水壶的持续浇水也有技能的分段浇水 需要用hold判定
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 计算分段进度动作的伤害 比如采集砍树等
    /// </summary>
    /// <param name="fromEntity">攻击者</param>
    /// <param name="targetObject">目标农场对象</param>
    /// <param name="targetCurAction">使用的目标动作</param>
    /// <param name="drSkill"></param>
    /// <param name="seedData">随机数据 默认null</param>
    /// <returns></returns>
    public static (float damage, bool isCrit) CalculateSegmentProgressDamage(EntityBase fromEntity, ICollectResourceCore targetObject, eAction targetCurAction, DRSkill drSkill, int fromEntityLevel, InputRandomData seedData = null)
    {
        float damage;
        bool isCrit = false;
        // if (targetCurAction == eAction.Hoeing)
        // {
        //     damage = _attributeData.GetRealValue(eAttributeType.HoeingEffect) * _drSkill.HomeAttRate * TableDefine.THOUSANDTH_2_FLOAT;//技能倍率;
        // }else
        if (targetCurAction == eAction.Watering)
        {
            damage = fromEntity.EntityAttributeData.GetRealValue(eAttributeType.WateringEffect) * drSkill.HomeAttRate * TableDefine.THOUSANDTH_2_FLOAT;//技能倍率;
        }
        else
        {
            EntityAttributeData toAttribute = null;
            if ((targetCurAction & COLLECT_RESOURCE_ACTION_MASK) != 0)//是采集动作
            {
                if (targetObject is not HomeResourcesCore homeResource)
                {
                    throw new System.Exception($"ExecuteSegmentProgressAction targetObject is not HomeResourceCore,action={targetCurAction} type={targetObject.GetType()}");
                }

                toAttribute = homeResource.GetComponent<EntityAttributeData>();
            }

            (float calculateDamage, bool crit) = SkillDamage.CalculateHomeDamage(targetCurAction, fromEntity.EntityAttributeData, toAttribute, drSkill.HomeAttRate * TableDefine.THOUSANDTH_2_FLOAT, seedData, fromEntityLevel, targetObject.GetActionLevel(targetCurAction));
            damage = calculateDamage;
            isCrit = crit;
        }

        return (damage, isCrit);
    }
}