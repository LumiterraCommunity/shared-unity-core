

using GameMessageCore;
using UnityEngine;

public static class CaptureUtil
{
    /// <summary>
    /// 获取 线经过球体的交点 且距离相机更远的点
    /// </summary>
    /// <param name="sphereCentre">球心位置</param>
    /// <param name="sphereRadius">球半径</param>
    /// <param name="lineRay">直线射线</param>
    /// <returns></returns>
    public static UnityEngine.Vector3? LineIntersectSphereFartherPoint(Camera camera, UnityEngine.Vector3 sphereCentre, float sphereRadius, Ray lineRay)
    {
        (UnityEngine.Vector3? point1, UnityEngine.Vector3? point2) = MathUtilCore.LineIntersectSphere(sphereCentre, sphereRadius, lineRay);
        if (point1 != null && point2 != null)
        {
            float dis1 = UnityEngine.Vector3.Distance((UnityEngine.Vector3)point1, camera.gameObject.transform.position);
            float dis2 = UnityEngine.Vector3.Distance((UnityEngine.Vector3)point2, camera.gameObject.transform.position);
            return dis1 > dis2 ? point1 : point2;
        }

        return null;
    }


    /// <summary>
    /// 把瞄准点限定在球体半径内
    /// </summary>
    /// <param name="aimPoint">瞄准点</param>
    /// <param name="sphereCentre">球心</param>
    /// <param name="sphereRadius">球半径</param>
    /// <returns></returns>
    public static UnityEngine.Vector3 GetAimPointLimitRadius(UnityEngine.Vector3 aimPoint, UnityEngine.Vector3 sphereCentre, float sphereRadius)
    {
        float distance = UnityEngine.Vector3.Distance(aimPoint, sphereCentre);
        // 距离超过了球半径，取该方向的最大值
        if (distance > sphereRadius)
        {
            return sphereCentre + (aimPoint - sphereCentre).normalized * sphereRadius;
        }
        else
        {
            return aimPoint;
        }
    }

    /// <summary>
    /// 从两个实体中获取 抓捕的角色实体id和被抓捕的怪物id
    /// </summary>
    /// <param name="entity1"></param>
    /// <param name="entity2"></param>
    /// <returns></returns>
    public static (long, long, EntityBase, EntityBase) GetCaptureFromToIds(EntityBase entity1, EntityBase entity2)
    {
        if (entity1 == null && entity2 == null)
        {
            return (BattleDefine.ENTITY_ID_UNKNOWN, BattleDefine.ENTITY_ID_UNKNOWN, null, null);
        }

        EntityBase fromEntity = null;
        EntityBase toEntity = null;
        if (entity1 != null)
        {
            if (entity1.BaseData.Type == EntityType.Player)
            {
                fromEntity = entity1;
                toEntity = entity2;
            }
            else
            {
                fromEntity = entity2;
                toEntity = entity1;
            }
        }
        else if (entity2 != null)
        {
            if (entity2.BaseData.Type == EntityType.Player)
            {
                fromEntity = entity2;
                toEntity = entity1;
            }
            else
            {
                fromEntity = entity1;
                toEntity = entity2;
            }
        }

        long fromId = BattleDefine.ENTITY_ID_UNKNOWN;
        if (fromEntity != null && fromEntity.BaseData != null)
        {
            fromId = fromEntity.BaseData.Id;
        }
        else if (toEntity != null && toEntity.CaptureData != null)
        {
            fromId = toEntity.CaptureData.CaptureId;
        }

        long toId = BattleDefine.ENTITY_ID_UNKNOWN;
        if (fromEntity != null && fromEntity.CaptureData != null)
        {
            toId = fromEntity.CaptureData.CaptureId;
        }
        else if (toEntity != null && toEntity.BaseData != null)
        {
            toId = toEntity.BaseData.Id;
        }

        return (fromId, toId, fromEntity, toEntity);
    }
}