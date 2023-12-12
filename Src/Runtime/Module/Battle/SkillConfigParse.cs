using System.Collections.Generic;
using UnityGameFramework.Runtime;

/** 
 * @Author XQ
 * @Date 2022-08-11 11:54:34
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/SkillConfigParse.cs
 */

/// <summary>
/// 技能配置解析
/// </summary>
public static class SkillConfigParse
{
    /// <summary>
    /// 解析技能效果
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="formID"></param>
    /// <param name="targetID"></param>
    /// <param name="effectList"></param>
    /// <returns></returns>
    public static List<SkillEffectBase> ParseSkillEffect(int skillID, long formID, long targetID, int[] effectList)
    {
        List<SkillEffectBase> skillEffects = new();
        if (effectList == null || effectList.Length <= 0)
        {
            return skillEffects;
        }

        for (int i = 0; i < effectList.Length; i++)
        {
            int effectID = effectList[i];
            DRSkillEffect skillEffectCfg = GFEntryCore.DataTable.GetDataTable<DRSkillEffect>().GetDataRow(effectID);
            if (skillEffectCfg == null)
            {
                Log.Error($"not find skill effect skillID = {skillID} effectID = {effectID}");
                continue;
            }
            try
            {
                SkillEffectBase skillBase = GFEntryCore.SkillEffectFactory.CreateOneSkillEffect(skillID, effectID, formID, targetID, skillEffectCfg.Duration);
                skillEffects.Add(skillBase);
            }
            catch (System.Exception)
            {
                Log.Error($"ParseSkillEffect error = {skillID} effectID = {effectID}");
            }
        }
        return skillEffects;
    }
}