using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

/// <summary>
/// 自定义的一些数学工具
/// </summary>
public static class MapUtilCore
{
    private const float HALF_FLOAT_MAX = float.MaxValue / 2;
    public static Collider[] OverlapColliderArray = new Collider[10];

    /// <summary>
    /// 和地面交点
    /// </summary>
    /// <param name="plane">地面高</param>
    /// <returns></returns>
    public static Vector3 GetPlaneInteractivePoint(Ray ray, float plane = 0)
    {
        Vector3 dir = ray.direction;

        if (Mathf.Approximately(dir.y, 0))
        {
            return Vector3.zero;
        }

        float num = (plane - ray.origin.y) / dir.y;
        return ray.origin + (ray.direction * num);
    }

    /// <summary>
    /// 根据给定的位置，获取离位置最近的课可行走位置
    /// </summary>
    /// <param name="position"></param>
    /// <param name="walkablePos">地表可行走结果点</param>
    /// <param name="maxError"></param>
    /// <returns>寻路异常返回false out原始位置</returns>
    public static bool SampleTerrainWalkablePos(Vector3 position, out Vector3 walkablePos, float maxError = 10f)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, maxError, NavMesh.AllAreas))
        {
            walkablePos = position;
            return false;
        }

        walkablePos = hit.position;
        return true;
    }

    /// <summary>
    /// 获取某个位置下面的贴地表的位置
    /// </summary>
    /// <param name="pos">从这个点开始向下检查</param>
    /// <param name="resPos">结果位置，失败返回当前位置</param>
    /// <param name="testDistance">测试距离 默认无穷大</param>
    /// <returns>成功找到返回true</returns>
    public static bool SamplePosOnTerrain(Vector3 pos, out Vector3 resPos, float testDistance = float.MaxValue)
    {
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, testDistance, MLayerMask.MASK_SCENE_OBSTRUCTION, QueryTriggerInteraction.Ignore))
        {
            resPos = hit.point;
            return true;
        }

        resPos = pos;
        return false;
    }

    /// <summary>
    /// x、z水平二维坐标下面的贴地表的位置
    /// </summary>
    /// <param name="resPos">结果位置 失败返回水平位置</param>
    /// <param name="testDistance">测试距离 默认无穷大</param>
    /// <returns>成功找到返回true</returns>
    public static bool SamplePosOnTerrain(float x, float z, out Vector3 resPos, float testDistance = float.MaxValue)
    {
        if (Physics.Raycast(new Vector3(x, HALF_FLOAT_MAX, z), Vector3.down, out RaycastHit hit, testDistance, MLayerMask.MASK_SCENE_OBSTRUCTION, QueryTriggerInteraction.Ignore))
        {
            resPos = hit.point;
            return true;
        }

        resPos = new Vector3(x, 0, z);
        return false;
    }

    /// <summary>
    /// 客户端场景名转服务器场景名
    /// </summary>
    /// <param name="sceneName">客户端场景名</param>
    /// <returns></returns>
    public static string SceneNameC2S(string sceneName)
    {
        return $"{sceneName}Server";
    }

    /// <summary>
    /// 获取一个有效的资源空地点，如果找不到返回null
    /// </summary> 
    /// <param name="pos">中心点</param>
    /// <param name="distance">向下距离</param> 
    /// <param name="maxError">最大误差</param>
    public static Vector3? GetValidEmptyAreaPos(Vector3 pos, float distance, float maxError)
    {
        _ = SamplePosOnTerrain(pos, out Vector3 terrainPos, distance);
        if (SampleTerrainWalkablePos(terrainPos, out Vector3 walkablePos, maxError))
        {
            //检测是否在遮挡物里面, 创建一个半径为0.1f的球体检测碰撞，向上偏移0.5f，防止接触到地面
            Vector3 checkPos = new(walkablePos.x, walkablePos.y + 0.5f, walkablePos.z);
            int terrainNum = Physics.OverlapSphereNonAlloc(checkPos, 0.1f, OverlapColliderArray, MLayerMask.MASK_SCENE_OBSTRUCTION | (1 << MLayerMask.WATER), QueryTriggerInteraction.Ignore);
            //有碰撞体，说明有遮挡物
            if (terrainNum > 0)
            {
                return null;
            }
            return walkablePos;
        }
        return null;
    }
}