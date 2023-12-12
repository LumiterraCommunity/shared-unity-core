/*
 * @Author: xiang huan
 * @Date: 2023-07-26 16:01:51
 * @Description: 检测应用效果球，更新层级
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFunApplySkillEffect.cs
 * 
 */


using UnityGameFramework.Runtime;

public class SEFunApplySkillEffect : SEFuncUpdateLayer
{
    public enum eEventKey
    {
        EffectId = 1,
        EffectType
    }

    protected eEventKey EventKey; //
    protected int EffectKey; //效果Key
    protected int AddLayerNum; //增加层级数
    public override void Init()
    {
        base.Init();
        int[] parameters = (int[])FuncData;
        if (parameters == null || parameters.Length != 4)
        {
            Log.Error($"SEFunApplySkillEffect FuncData error SkillEffectID = {SkillEffect.EffectID} ");
            return;
        }
        EventKey = (eEventKey)parameters[1];
        EffectKey = parameters[2];
        AddLayerNum = parameters[3];
    }
    public override void Clear()
    {
        EventKey = eEventKey.EffectId;
        EffectKey = 0;
        AddLayerNum = 0;
        base.Clear();
    }
    protected override void OnAddEvent()
    {
        base.OnAddEvent();
        RefEntity.EntityEvent.AfterApplySkillEffect += OnAfterApplySkillEffect;
    }

    protected override void OnRemoveEvent()
    {
        RefEntity.EntityEvent.AfterApplySkillEffect -= OnAfterApplySkillEffect;
        base.OnRemoveEvent();
    }

    private void OnAfterApplySkillEffect(GameMessageCore.DamageEffect effect)
    {
        DRSkillEffect skillEffectCfg = GFEntryCore.DataTable.GetDataTable<DRSkillEffect>().GetDataRow((int)effect.EffectType);
        if (skillEffectCfg == null)
        {
            Log.Error($"SEFunApplySkillEffect Error skillEffectCfg is null effectID = {effect.EffectType}");
            return;
        }
        int curValue = SkillEffect.CurLayer;
        if (EventKey == eEventKey.EffectId)
        {
            if (skillEffectCfg.Id == EffectKey)
            {
                curValue += AddLayerNum;
            }
        }
        else
        {
            if (skillEffectCfg.EffectType == EffectKey)
            {
                curValue += AddLayerNum;
            }
        }
        UpdateLayer(curValue);
    }


}