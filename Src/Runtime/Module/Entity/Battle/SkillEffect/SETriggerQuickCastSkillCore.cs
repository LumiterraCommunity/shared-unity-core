using System.Collections.Generic;
/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 触发快速释放技能
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SETriggerQuickCastSkillCore.cs
* 
*/

using UnityGameFramework.Runtime;

public class SETriggerQuickCastSkillCore : SkillEffectBase
{
    protected HashSet<int> TriggerIDMap = new();
    protected int TriggerType;
    protected int TriggerRate;
    protected int CastSkillID;

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        if (EffectCfg.Parameters2 == null || EffectCfg.Parameters2.Length != 2)
        {
            Log.Error($"SETriggerQuickCastSkillCore Parameters2 Error EffectID = {EffectID}");
            return;
        }
        for (int i = 0; i < EffectCfg.Parameters2[0].Length; i++)
        {
            _ = TriggerIDMap.Add(EffectCfg.Parameters2[0][i]);
        }

        TriggerType = EffectCfg.Parameters2[1][0];
        TriggerRate = EffectCfg.Parameters2[1][1];
        CastSkillID = EffectCfg.Parameters2[1][2];

    }

    public override void OnRemove()
    {
        TriggerIDMap.Clear();
        base.OnRemove();
    }
}