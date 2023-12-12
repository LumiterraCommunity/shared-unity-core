using UnityEngine;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

/// <summary>
/// 场景破坏元模块
/// </summary>
/// <typeparam name="TElement"></typeparam>
public class SceneDestructionModuleCore<TElement> : MonoBehaviour where TElement : DestructionElementCore
{
    private readonly Dictionary<long, TElement> _elementMap = new();

    /// <summary>
    /// 添加一个破坏元素的管理引用
    /// </summary>
    /// <param name="id"></param>
    /// <param name="element"></param>
    public void AddElement(long id, TElement element)
    {
        if (_elementMap.ContainsKey(id))
        {
            Log.Error($"SceneDestructionModuleCore AddElement id:{id} already exist");
            return;
        }

        _elementMap.Add(id, element);
    }

    /// <summary>
    /// 移除一个破坏元素的管理引用 不会执行销毁 
    /// </summary>
    /// <param name="id"></param>
    public void RemoveElement(long id)
    {
        if (!_elementMap.ContainsKey(id))
        {
            Log.Error($"SceneDestructionModuleCore RemoveElement id:{id} not exist");
            return;
        }

        _ = _elementMap.Remove(id);
    }

    /// <summary>
    /// 获取一个破坏元素 如果不存在返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TElement GetElement(long id)
    {
        return !_elementMap.ContainsKey(id) ? null : _elementMap[id];
    }

    /// <summary>
    /// 清理管理的引用 不会执行销毁对象
    /// </summary>
    public void Clear()
    {
        _elementMap.Clear();
    }
}