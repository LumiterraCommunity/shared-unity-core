using UnityEngine;
using UnityGameFramework.Runtime;
/// <summary>
/// 区域入口检测器
/// </summary>

[ExecuteAlways]
public class SceneAreaEntryChecker : MonoBehaviour
{
    /// <summary>
    /// 区域标识
    /// </summary>
    public eSceneArea Area = eSceneArea.World;
    /// <summary>
    /// 场景区域优先级
    /// 用于区域重叠时的优先级判断
    /// 枚举值越大，优先级越高，越先触发 
    /// 注意:进入重叠区域的时候，最终触发的是优先级最高的区域，但是最终确定进入的区域是重叠中优先级最低的区域
    /// </summary>
    [Header("区域重叠的时候，优先级越大，越先进入")]
    public eSceneAreaPriority Priority = eSceneAreaPriority.none;

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            //避免铺地图的时候忘记设置tag
            gameObject.tag = MTag.SCENE_AREA_CHECKER;
            gameObject.layer = MLayerMask.PUBLIC_TRIGGER;
        }
#endif
        if (TryGetComponent(out Collider collider))
        {
            if (!collider.isTrigger)
            {
                Log.Warning("collider must be trigger");
                collider.isTrigger = true;
            }
        }
        else
        {
            Log.Error("SceneEntryChecker must have a collider component.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }

        EntityBase entity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(other.gameObject);
        if (entity != null && entity.Inited)
        {
            OnPlayerEnterNewSceneArea(entity, Area);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }

        EntityBase entity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(other.gameObject);
        if (entity != null && entity.Inited)
        {
            OnPlayerExitCurSceneArea(entity, Area);
        }
    }

    private void OnPlayerEnterNewSceneArea(EntityBase entity, eSceneArea newArea)
    {
        long playerID = entity.BaseData.Id;
        Log.Info($"OnPlayerEnterNewScene,{gameObject.name} {playerID}, {newArea}");
        SceneAreaInfo info = new(newArea, Priority, GetHashCode());
        PlayerAreaRecord record = entity.GetOrAddComponent<PlayerAreaRecord>();
        record.ReceiveAreaChangedEvent(info, eAreaChangedType.enter);
    }

    private void OnPlayerExitCurSceneArea(EntityBase entity, eSceneArea newArea)
    {
        long playerID = entity.BaseData.Id;
        Log.Info($"OnPlayerExitCurScene, {gameObject.name} {playerID}, {newArea}");
        SceneAreaInfo info = new(newArea, Priority, GetHashCode());
        PlayerAreaRecord record = entity.GetOrAddComponent<PlayerAreaRecord>();
        record.ReceiveAreaChangedEvent(info, eAreaChangedType.exit);
    }

#if UNITY_EDITOR
    public bool DrawArea = true;
    public Color AreaColor = Color.green;
    private void OnDrawGizmos()
    {
        if (!DrawArea)
        {
            return;
        }
        //根据碰撞盒子绘制区域
        Gizmos.color = AreaColor;
        if (TryGetComponent(out Collider collider))
        {
            if (collider is BoxCollider box)
            {
                Gizmos.DrawWireCube(transform.position + box.center, new Vector3(box.size.x * transform.lossyScale.x, box.size.y * transform.lossyScale.y, box.size.z * transform.lossyScale.z));
            }
            else if (collider is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
            }
            else if (collider is CapsuleCollider capsule)
            {
                //to do
                // DrawCapsule(transform.position + capsule.center, capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z), capsule.height * transform.lossyScale.y, transform.rotation);
            }
            else if (collider is MeshCollider mesh)
            {
                Gizmos.DrawWireMesh(mesh.sharedMesh, transform.position, transform.rotation, transform.lossyScale);
            }
        }
    }

    // 绘制胶囊体线框
    private void DrawCapsule(Vector3 center, float radius, float height, Quaternion rotation)
    {
        Vector3 point1 = center + (rotation * new Vector3(0, height * 0.5f, 0));
        Vector3 point2 = center + (rotation * new Vector3(0, -height * 0.5f, 0));
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1 + (rotation * new Vector3(radius, 0, 0)), point2 + (rotation * new Vector3(radius, 0, 0)));
        Gizmos.DrawLine(point1 + (rotation * new Vector3(-radius, 0, 0)), point2 + (rotation * new Vector3(-radius, 0, 0)));
        Gizmos.DrawLine(point1 + (rotation * new Vector3(0, 0, radius)), point2 + (rotation * new Vector3(0, 0, radius)));
        Gizmos.DrawLine(point1 + (rotation * new Vector3(0, 0, -radius)), point2 + (rotation * new Vector3(0, 0, -radius)));
    }
#endif
}