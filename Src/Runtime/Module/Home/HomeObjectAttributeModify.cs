using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 家园对象上用来管理批量属性修改的组件 比如副本事件中修改家园土地的属性
/// </summary>
public class HomeObjectAttributeModify : MonoBehaviour
{
    private Dictionary<object, List<IntAttributeModifier>> _curModuleModifierMap;

    /// <summary>
    /// 外部模块通过配置表来批量添加属性修改器 返回是否添加成功
    /// </summary>
    /// <param name="module"></param>
    /// <param name="tableParm">配置表中二维int参数 eg:"15,4,200,15,4;13,4,100,13,4"</param>
    /// <returns></returns>
    public bool AddAttributeModifies(object module, int[][] tableParm)
    {
        if (module == null)
        {
            Log.Error($"HomeObjectAttributeModify AddAttributeModifies module==null");
            return false;
        }

        if (tableParm == null || tableParm.Length == 0)
        {
            return false;
        }

        //组件会动态添加删除 比如只有种子后才有属性组件 需要检查
        if (!TryGetComponent(out AttributeDataCpt attributeCpt))
        {
            return false;
        }

        _curModuleModifierMap ??= new Dictionary<object, List<IntAttributeModifier>>();
        if (_curModuleModifierMap.ContainsKey(module))
        {
            Log.Error($"HomeObjectAttributeModify AddAttributeModifies module exist module:{module.GetType().Name}");
            return false;
        }

        List<IntAttributeModifier> modifiers = TableUtil.GenerateAttributeModify(attributeCpt, null, tableParm);
        _curModuleModifierMap.Add(module, modifiers);

        OnAttributeChanged(attributeCpt);
        return true;
    }

    public void RemoveAttributeModifies(object module)
    {
        if (module == null)
        {
            Log.Error($"HomeObjectAttributeModify RemoveAttributeModifies module==null");
            return;
        }

        if (_curModuleModifierMap == null || !_curModuleModifierMap.TryGetValue(module, out var modifiers))
        {
            Log.Error($"HomeObjectAttributeModify RemoveAttributeModifies not find module:{module.GetType().Name}");
            return;
        }

        //组件会动态添加删除 比如只有种子后才有属性组件 需要检查
        if (TryGetComponent(out AttributeDataCpt attributeCpt))
        {
            foreach (IntAttributeModifier modifier in modifiers)
            {
                attributeCpt.RemoveModifier(modifier);
            }
            OnAttributeChanged(attributeCpt);
        }

        _curModuleModifierMap.Remove(module);

        if (_curModuleModifierMap.Count == 0)
        {
            _curModuleModifierMap = null;
        }
    }

    /// <summary>
    /// 属性已经变化了 子类选择性重写
    /// </summary>
    protected virtual void OnAttributeChanged(AttributeDataCpt attributeCpt) { }
}