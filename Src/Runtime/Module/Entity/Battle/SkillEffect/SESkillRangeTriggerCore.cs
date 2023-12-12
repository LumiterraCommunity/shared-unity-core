/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 技能范围触发效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SESkillRangeTriggerCore.cs
* 
*/


using UnityGameFramework.Runtime;

public class SESkillRangeTriggerCore : SkillEffectBase
{

    protected int TriggerType;
    protected int MaxTriggerNum;
    protected int[] TriggerEffectList;
    protected DRSkill CurSkillCfg;
    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }
        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        if (EffectCfg.Parameters2 == null || EffectCfg.Parameters2.Length != 2)
        {
            Log.Error($"SESkillRangeTriggerCore Parameters2 Error EffectID = {EffectID}");
            return;
        }


        TriggerType = EffectCfg.Parameters2[0][0];
        MaxTriggerNum = EffectCfg.Parameters2[0][1];
        TriggerEffectList = EffectCfg.Parameters2[1];
        CurSkillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(SkillID);
    }
}