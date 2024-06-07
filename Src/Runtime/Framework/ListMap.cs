using System;
using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

/// <summary>
/// 具有List的遍历性能和Map的查找性能的 for和foreach中当作list使用 下标也是list使用 频繁增删的性能和List一致 foreach性能如List for中this如list
/// !! 为了节省创建遍历器的开销 不支持同对象嵌套foreach 和 GetEnumerator 否则会异常
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ListMap<TKey, TValue> : IEnumerator, IEnumerable
{
    private readonly List<TValue> _list = new();
    private readonly Dictionary<TKey, TValue> _dic = new();

    public int Count => _list.Count;

    /// <summary>
    /// 无GC，不要改变列表值
    /// </summary>
    public List<TValue> Values => _list;

    /// <summary>
    /// 返回字典中的Keys 这个和字典一样无法保证顺序
    /// </summary>
    public Dictionary<TKey, TValue>.KeyCollection Keys => _dic.Keys;

    /// <summary>
    /// 添加一个数据 key为id之类的唯一值 内部字典会采用该值作为key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Add(TKey key, TValue value)
    {
        if (_dic.ContainsKey(key))
        {
            Log.Error($"ListMap Add key already exist key:{key}");
            return false;
        }

        _dic.Add(key, value);
        _list.Add(value);
        return true;
    }

    public bool Remove(TKey key)
    {
        if (!_dic.TryGetValue(key, out TValue value))
        {
            Log.Error($"ListMap Remove key not exist key:{key}");
            return false;
        }

        _ = _dic.Remove(key);
        _ = _list.Remove(value);
        return true;
    }

    /// <summary>
    /// 设置一个数据 List的顺序会变化 以为value可能是值类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(TKey key, TValue value)
    {
        if (_dic.ContainsKey(key))
        {
            _ = Remove(key);
        }

        _ = Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return _dic.ContainsKey(key);
    }

    public void Clear()
    {
        _dic.Clear();
        _list.Clear();
    }

    /// <summary>
    /// 使用list index访问 不会保护越界
    /// </summary>
    public TValue this[int index] => _list[index];

    /// <summary>
    /// 通过index获取值 会保护越界
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public TValue GetFromIndex(int index)
    {
        if (index < 0 || index >= _list.Count)
        {
            Log.Error($"ListMap GetFromIndex index out of range index:{index}");
            return default;
        }

        return _list[index];
    }

    /// <summary>
    /// 通过key尝试获取 如同字典的TryGetValue
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValueFromKey(TKey key, out TValue value)
    {
        return _dic.TryGetValue(key, out value);
    }

    /// <summary>
    /// 通过key获取值 会保护key不存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue GetFromKey(TKey key)
    {
        if (!_dic.TryGetValue(key, out TValue value))
        {
            Log.Error($"ListMap GetFromKey key not exist key:{key}");
            return default;
        }

        return value;
    }

    #region  IEnumerator

    private int _index = -1;//记录当前迭代器的位置，初始为-1
    object IEnumerator.Current => _list[_index];

    bool IEnumerator.MoveNext()
    {
        _index++;
        if (_index < _list.Count)
        {
            return true;
        }
        return false;
    }

    void IEnumerator.Reset()
    {
        _index = -1;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        (this as IEnumerator).Reset();
        return this;
    }

    public TValue[] ToArray()
    {
        return _list.ToArray();
    }

    #endregion
}