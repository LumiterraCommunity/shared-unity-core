using UnityEngine;

public static class GameObjectExtension
{
    /// <summary>
    /// 创建一个相对位置0的子节点
    /// </summary>
    /// <param name="name">子节点名字 给空时使用系统的名字</param>
    /// <returns></returns>
    public static GameObject CreateChildNode(this GameObject cur, string name)
    {
        GameObject child = string.IsNullOrEmpty(name) ? new GameObject() : new GameObject(name);
        child.transform.SetParent(cur.transform, false);
        return child;
    }
}