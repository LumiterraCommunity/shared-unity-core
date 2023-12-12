using System;
using GameFramework;
using UnityGameFramework.Runtime;

/// <summary>
/// 任务委托 保存回调和参数 用于任务队列等地方
/// </summary>
public class TaskDelegate : IReference
{
    private Action<object> _callBack1;//有参数的回调
    private Action _callBack;//无参数的回调

    private object _parm;

    public void SetData(Action<object> cb, object parm)
    {
        _callBack1 = cb;
        _parm = parm;

        _callBack = null;
    }

    public void SetData(Action cb)
    {
        _callBack = cb;

        _callBack1 = null;
        _parm = null;
    }

    public void Execute()
    {
        try
        {
            if (_callBack != null)
            {
                _callBack?.Invoke();
            }
            else
            {
                _callBack1?.Invoke(_parm);
            }
        }
        catch (Exception e)
        {
            Log.Error("TaskDelegate execute error:\n" + e);
        }
    }

    public void Clear()
    {
        _callBack = null;
        _callBack1 = null;
        _parm = null;
    }
}
