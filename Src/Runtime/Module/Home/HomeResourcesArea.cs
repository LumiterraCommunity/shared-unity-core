/*
 * @Author: xiang huan
 * @Date: 2022-12-06 10:27:50
 * @Description: 资源刷新区域
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Home/HomeResourcesArea.cs
 * 
 */
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEditor;
[ExecuteAlways]
public class HomeResourcesArea : SharedCoreComponent
{
    [SerializeField]
    [Header("全局ID 自动生成 不要乱改")]
    private int _id;
    public int Id => _id;

    [Header("是否绘制形状")]
    public bool IsDraw;

    [Header("配置ID")]
    public int ConfigId;

    [Header("刷新关卡,控制资源刷新的关卡等级，0为不限制")]
    public int RefreshLevel = 0;

    public Bounds AreaBounds { get; private set; }
    public HomeResourcesAreaSaveData SaveData { get; private set; }  //保存数据

    public DRHomeResourceArea DRHomeResourceArea { get; private set; }
    private void Awake()
    {
#if UNITY_EDITOR
        //为了自动序列化ID和服务器场景同步使用
        if (!Application.isPlaying)
        {
            if (_id == default//直接在场景中添加组件
            || (PrefabUtility.GetCorrespondingObjectFromSource(this) != null && gameObject.scene != null && gameObject.scene.isLoaded))//发生在预制件拖动到场景中，场景加载时的awake不会走这里
            {
                AutoSetID();
            }
        }
#endif
        if (Application.isPlaying)
        {
            AreaBounds = new Bounds(transform.position, transform.localScale);
            DRHomeResourceArea = GFEntryCore.DataTable.GetDataTable<DRHomeResourceArea>().GetDataRow(ConfigId);
            if (DRHomeResourceArea == null)
            {
                Log.Error("HomeResourcesArea Awake Error: Can't find config id = " + ConfigId);
                return;
            }
            SaveData = CreateSaveData();
            GFEntryCore.HomeResourcesAreaMgr.AddArea(this);
        }
    }


    private void OnDestroy()
    {
        if (Application.isPlaying)
        {

            GFEntryCore.HomeResourcesAreaMgr.RemoveArea(Id);
        }
    }

    /// <summary>
    /// 创建存储数据
    /// </summary>
    protected HomeResourcesAreaSaveData CreateSaveData()
    {
        if (DRHomeResourceArea == null)
        {
            return null;
        }
        long updateTime = (GFEntryCore.GFEntryType == GFEntryType.Client ? TimeUtil.GetTimeStamp() : TimeUtil.GetServerTimeStamp()) + DRHomeResourceArea.UpdateInterval;
        if (DRHomeResourceArea.IsUpdateNow)
        {
            updateTime = 0;
        }
        HomeResourcesAreaSaveData data = new()
        {
            Id = Id,
            PointList = new(),
            //这里客户端使用自己的本地时间 因为这些是在加载场景时就加载就初始化了 没有同步服务器时间 而且看现在逻辑 也不用这个时间 只是和服务器共享了逻辑而已
            UpdateTime = updateTime
        };
        return data;
    }

    /// <summary>
    /// 初始化存储数据
    /// </summary>
    public void SetSaveData(HomeResourcesAreaSaveData saveData)
    {
        if (saveData == null)
        {
            return;
        }
        SaveData = saveData;
    }

    /// <summary>
    /// 获得存储数据
    /// </summary>
    public HomeResourcesAreaSaveData GetSaveData()
    {
        return SaveData;
    }

    /// <summary>
    /// 设置存储数据刷新时间
    /// </summary>
    public void SetSaveDataUpdateTime(long time)
    {
        if (SaveData == null)
        {
            return;
        }
        SaveData.UpdateTime = time;
    }

    /// <summary>
    /// 添加存储数据资源点
    /// </summary>
    public void AddSaveDataPoint(HomeResourcesPointSaveData pointData)
    {
        if (SaveData == null)
        {
            return;
        }
        SaveData.PointList.Add(pointData);
    }

    /// <summary>
    /// 删除存储数据资源点
    /// </summary>
    public void RemoveSaveDataPoint(ulong id)
    {
        if (SaveData == null)
        {
            return;
        }
        for (int i = 0; i < SaveData.PointList.Count; i++)
        {
            if (SaveData.PointList[i].Id == id)
            {
                SaveData.PointList.RemoveAt(i);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!IsDraw)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMatrix;
    }
#if UNITY_EDITOR
    protected virtual void Update()
    {
        UpdateID();
    }
#endif
#if UNITY_EDITOR
    private void UpdateID()
    {
        if (Application.isPlaying)
        {
            return;
        }

        CheckIDRevertFromPrefab();
    }

    /// <summary>
    // 检查是否从预制件中恢复ID成了预制件的ID  需要再改掉
    /// </summary>
    private void CheckIDRevertFromPrefab()
    {
        HomeResourcesArea prefabComponent = PrefabUtility.GetCorrespondingObjectFromSource(this);
        if (prefabComponent == null)
        {
            return;
        }

        if (_id == prefabComponent._id && _id != GetInstanceID())
        {
            AutoSetID();
        }
    }

    private void AutoSetID()
    {
        _id = GetInstanceID();//给定一个全局ID
        EditorUtility.SetDirty(this);
    }
#endif
}