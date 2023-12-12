using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 以固定时间跟随到达
/// </summary>
public class FollowArriveFromTime : ArriveAutoDestroy
{
    public Transform Target { get; private set; }
    private Vector3 _offset = Vector3.zero;
    private Tweener _tweener;

    /// <summary>
    /// 开始跟随
    /// </summary>
    /// <param name="target"></param>
    /// <param name="costTime">多久后到达 秒</param>
    /// <param name="offset">跟随的目标点相对于目标的偏移量 默认为null</param>
    public void StartMove(Transform target, float costTime, Vector3? offset = null)
    {
        if (target == null)
        {
            Log.Error("FollowArriveFromTime target is null.");
            CancelArrive();
            return;
        }

        Target = target;
        _offset = offset == null ? Vector3.zero : offset.Value;

        if (costTime <= 0)
        {
            Log.Error("FollowArriveFromTime cost time is zero.");
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

        _tweener = transform.DOMove(Target.position + _offset, costTime).SetEase(Ease.Linear).OnComplete(() =>
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

        //目标可能中途销毁
        if (Target == null)
        {
            CancelArrive();
            return;
        }

        float remainTime = _tweener.Duration() - _tweener.position;
        _ = _tweener.ChangeEndValue(Target.position + _offset, remainTime, true);
        transform.forward = (Target.position - transform.position).OnlyXZ();//水平旋转即可
    }
}