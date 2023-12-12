/*
 * @Author: xiang huan
 * @Date: 2023-07-03 14:50:52
 * @Description: 子弹抛物线位移
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Battle/Flyer/ParabolaArriveFromTime.cs
 * 
 */
using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 以固定时间跟随到达
/// </summary>
public class ParabolaArriveFromTime : ArriveAutoDestroy
{
    private Vector3 _targetPos;
    private float _height;
    private Vector3 _offset = Vector3.zero;
    private Tweener _tweener;
    private Vector3 _lastPos;

    /// <summary>
    /// 开始跟随
    /// </summary>
    /// <param name="target"></param>
    /// <param name="costTime">多久后到达 秒</param>
    /// <param name="offset">跟随的目标点相对于目标的偏移量 默认为null</param>
    public void StartMove(Vector3 targetPos, float costTime, float height, Vector3? offset = null)
    {
        _targetPos = targetPos;
        _height = height;
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
        Vector3 startPos = transform.position;
        _lastPos = startPos;
        float startValue = 0f;
        float endValue = 1f;
        _tweener = DOTween.To(() => startValue, value =>
        {
            Vector3 curPos = MathUtilCore.Parabola(startPos, _targetPos, _height, value);
            transform.position = curPos;
            transform.forward = curPos - _lastPos;
            _lastPos = curPos;
        }, endValue, costTime).SetEase(Ease.Linear).OnComplete(() =>
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