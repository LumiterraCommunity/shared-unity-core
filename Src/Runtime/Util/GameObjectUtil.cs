using UnityEngine;

/// <summary>
/// 关于unity的GameObject相关的工具
/// </summary>
public static class GameObjectUtil
{
    /// <summary>
    /// 创建一个空对象 如果设置了父节点会设置坐标0
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject CreateGameObject(string name, Transform parent = null)
    {
        GameObject go = new(name);
        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
        }
        return go;
    }
}