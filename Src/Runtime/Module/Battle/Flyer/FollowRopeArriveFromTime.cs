using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 在我方与己方的对角线上 以固定时间跟随到达 （表现为沿着绳子运动）
/// </summary>
public class FollowRopeArriveFromTime : ArriveAutoDestroy
{
    private Tweener _tweener;

    public void StartMove(RoleBaseDataCore fromEntityData, RoleBaseDataCore toEntityData, float costTime)
    {
        if (costTime <= 0)
        {
            Log.Error("FollowArriveFromTime cost time is zero.");
            OnArrived();
            return;
        }

        StartTween(fromEntityData, toEntityData, costTime);
    }

    private void StartTween(RoleBaseDataCore fromEntityData, RoleBaseDataCore toEntityData, float costTime)
    {
        if (_tweener != null)
        {
            StopTween();
        }
        float x = 0f;
        _tweener = DOTween.To(() => x, (rate) =>
            {
                x = rate;
                Vector3 dir = (toEntityData.CenterPos - fromEntityData.CenterPos).normalized;
                transform.position = fromEntityData.CenterPos + (toEntityData.CenterPos - fromEntityData.CenterPos) * rate;
                transform.LookAt(toEntityData.CenterPos);
            }, 1, costTime).SetEase(Ease.OutQuint).OnComplete(() =>
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