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
                    List<IntAttributeModifier> list = TableUtil.GenerateAttributeModify(RefEntity.EntityAttributeData, EffectCfg.Parameters2, out eAttributeType excludeType, out int excludeValue);
                    if (excludeType == eAttributeType.HP)
                    {
                        RefEntity.BattleDataCore.SetHP(RefEntity.BattleDataCore.HP + excludeValue);
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