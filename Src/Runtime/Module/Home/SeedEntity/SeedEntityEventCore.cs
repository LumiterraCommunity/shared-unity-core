using System;
using UnityEngine;

/// <summary>
/// 种子实体自身的事件
/// </summary>
public class SeedEntityEventCore : MonoBehaviour
{
    /// <summary>
    /// 初始化完成时的事件
    /// </summary>
    public Action OnInitFinished;
    /// <summary>
    /// 实体释放时的事件
    /// </summary>
    public Action OnEntityRemoved;
}