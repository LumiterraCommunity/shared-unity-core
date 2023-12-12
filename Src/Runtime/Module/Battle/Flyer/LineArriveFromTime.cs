
/*
 * @Author: xiang huan
 * @Date: 2023-07-03 14:50:52
 * @Description: 子弹直线位移
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/Flyer/LineArriveFromTime.cs
 * 
 */
using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 以固定时间跟随到达
/// </summary>
public class LineArriveFromTime : ArriveAutoDestroy
{
    private Vector3 _targetPos;
    private Vector3 _offset = Vector3.zero;
    private Tweener _tweener;

    /// <summary>
    /// 开始跟随
    /// </summary>
    /// <param name="target"></param>
    /// <param name="costTime">多久后到达 秒</param>
    /// <param name="offset">跟随的目标点相对于目标的偏移量 默认为null</param>
    public void StartMove(Vector3 targetPos, float costTime, Vector3? offset = null)
    {
        _targetPos = targetPos;
        _offset = offset == null ? Vector3.zero : offset.Value;

        if (costTime <= 0)
        {
            Log.Error("LineArriveFromTime cost time is zero.");
            OnArrived();
            return;
        }

        StartTween(costTime);
    }

    private void StartTween(float costTime)
    {
        if (_tweener != null)
        {
            StopTween();
        }

        _tweener = transform.DOMove(_targetPos + _offset, costTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            StopTween();
            OnArrived();
        });

    }

    private void OnDestroy()
    {
        StopTween();
    }

    private void StopTween()
    {
        if (_tweener == null)
        {
            return;
        }

        _tweener.Kill();
        _tweener = null;
    }
    private void Update()
    {
        if (_tweener == null)
        {
            return;
        }
        transform.LookAt(_targetPos);
    }
}