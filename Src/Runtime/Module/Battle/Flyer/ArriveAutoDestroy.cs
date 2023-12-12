using System;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 到达自动销毁
/// </summary>
public class ArriveAutoDestroy : MonoBehaviour
{
    /// <summary>
    /// 到达事件
    /// </summary>
    public event EventHandler ArrivedEvent;

    /// <summary>
    /// 到达 子类调用
    /// </summary>
    protected void OnArrived()
    {
        try
        {
            ArrivedEvent?.Invoke(this, null);
        }
        catch (System.Exception e)
        {
            Log.Error($"ArriveAutoDestroy OnArrived error. :{e}");
        }

        DestroyLogic();
    }

    /// <summary>
    /// 由于异常等中途取消到达逻辑 子类调用
    /// </summary>
    protected void CancelArrive()
    {
        DestroyLogic();
    }

    private void DestroyLogic()
    {
        Destroy(this);//自我销毁
    }
}