/*
 * @Author: xiang huan
 * @Date: 2023-07-26 16:01:51
 * @Description: 检测受到伤害，更新层级
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFuncAddDamage.cs
 * 
 */


using UnityGameFramework.Runtime;

public class SEFuncAddDamage : SEFuncUpdateLayer
{
    public enum eEventKey
    {
        Count = 1,
        DamageValue
    }

    protected eEventKey EventKey; //技能ID, -1为任意技能
    protected int AddLayerNum; //增加层级数
    public override void Init()
    {
        base.Init();
        int[] parameters = (int[])FuncData;
        if (parameters == null || parameters.Length != 3)
        {
            Log.Error($"SEFuncAddDamage FuncData error SkillEffectID = {SkillEffect.EffectID} ");
            return;
        }
        EventKey = (eEventKey)parameters[1];
        AddLayerNum = parameters[2];
    }
    public override void Clear()
    {
        EventKey = eEventKey.Count;
        AddLayerNum = 0;
        base.Clear();
    }
    protected override void OnAddEvent()
    {
        base.OnAddEvent();
        RefEntity.EntityEvent.EntityBattleAddDamage += OnEntityBattleAddDamage;
    }

    protected override void OnRemoveEvent()
    {
        RefEntity.EntityEvent.EntityBattleAddDamage -= OnEntityBattleAddDamage;
        base.OnRemoveEvent();
    }

    private void OnEntityBattleAddDamage(long targetId, int damageValue)
    {
        int curValue = SkillEffect.CurLayer;
        if (EventKey == eEventKey.DamageValue)
        {
            curValue += damageValue;
        }
        else
        {
            curValue += AddLayerNum;
        }
        UpdateLayer(curValue);
    }

}