using System.Collections.Generic;
/*
 * @Author: xiang huan
 * @Date: 2023-01-11 19:15:17
 * @Description: 属性修改
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEAttributeModifierCore.cs
 * 
 */

/// <summary>
/// 属性修改
/// </summary>
public class SEAttributeModifierCore : SkillEffectBase
{
    protected Dictionary<int, List<IntAttributeModifier>> ModifierMap = new();
    public override void UpdateLayer(int layer)
    {
        base.UpdateLayer(layer);

        if (RefEntity.EntityAttributeData != null)
        {
            //刷新buff层数
            for (int i = 1; i <= layer; i++)
            {
                if (!ModifierMap.ContainsKey(i))
                {
                    List<IntAttributeModifier> list = new();
                    for (int index = 0; index < EffectCfg.Parameters2.Length; index++)
                    {
                        eAttributeType attributeType = (eAttributeType)EffectCfg.Parameters2[index][0];
                        eModifierType modifierType = (eModifierType)EffectCfg.Parameters2[index][1];
                        int value = EffectCfg.Parameters2[index][2];
                        //基于其它属性进行加成计算
                        if (EffectCfg.Parameters2[index].Length > 4)
                        {
                            eAttributeType addType = (eAttributeType)EffectCfg.Parameters2[index][3]; //加成属性类型
                            eModifierType addModifierType = (eModifierType)EffectCfg.Parameters2[index][4]; //加成类型
                            int addValue;
                            if (addModifierType == eModifierType.PctAdd)
                            {
                                addValue = RefEntity.EntityAttributeData.GetBaseValue(addType);
                                value = (int)(addValue * value / IntAttribute.PERCENTAGE_FLAG);
                            }
                            else if (addModifierType == eModifierType.FinalPctAdd)
                            {
                                addValue = RefEntity.EntityAttributeData.GetValue(addType);
                                value = (int)(addValue * value / IntAttribute.PERCENTAGE_FLAG);
                            }
                        }
                        //血量做特殊处理，
                        if (attributeType == eAttributeType.HP)
                        {
                            RefEntity.BattleDataCore.SetHP(RefEntity.BattleDataCore.HP + value);
                        }
                        else
                        {
                            IntAttributeModifier modifier = RefEntity.EntityAttributeData.AddModifier(attributeType, modifierType, value);
                            list.Add(modifier);
                        }
                    }
                    ModifierMap.Add(i, list);
                }
            }

        }
    }
    public override void OnRemove()
    {
        if (RefEntity.EntityAttributeData != null)
        {
            foreach (KeyValuePair<int, List<IntAttributeModifier>> item in ModifierMap)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    RefEntity.EntityAttributeData.RemoveModifier(item.Value[i]);
                }
            }

            ModifierMap.Clear();
        }

        base.OnRemove();
    }
    public override void Clear()
    {

        base.Clear();
    }
}