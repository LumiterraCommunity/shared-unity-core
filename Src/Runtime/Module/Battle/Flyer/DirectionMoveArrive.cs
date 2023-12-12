using System;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 固定方向以固定速度、时间到达
/// </summary>
public class DirectionMoveArrive : ArriveAutoDestroy
{
    private Tweener _tweener;

    /// <summary>
    /// 开始移动
    /// </summary>
    /// <param name="toPos">目标位置</param>
    /// <param name="costTime">消耗时间</param>
    public void StartMove(Vector3 toPos, float costTime)
    {
        StartTween(toPos, costTime);
    }

    private void StartTween(Vector3 targetPos, float costTime)
    {
        if (_tweener != null)
        {
            StopTween();
        }

        _tweener = transform.DOMove(targetPos, costTime).SetEase(Ease.OutQuint).OnComplete(() =>
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
}