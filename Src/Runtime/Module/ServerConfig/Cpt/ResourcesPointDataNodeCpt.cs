/*
 * @Author: xiang huan
 * @Date: 2022-06-27 14:13:48
 * @Description: 资源点数据组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/ResourcesPointDataNodeCpt.cs
 * 
 */
using UnityEngine;
public class ResourcesPointDataNodeCpt : MonoBehaviour, IServerDataNodeCpt
{
    [Tooltip("资源类型1. monster  2.复活点 3.npc")]
    [Header("资源类型")]
    [SerializeField]
    private int _resourceType;
    public int ResourceType => _resourceType;
    [Header("配置ID")]
    [SerializeField]
    private int _configId;
    public int ConfigId => _configId;

    [Header("等级范围")]
    [SerializeField]
    private string _levelRange;
    public string LevelRange => _levelRange;

    [Header("刷新间隔时间(ms)")]
    [SerializeField]
    private int _updateInterval;
    public int UpdateInterval => _updateInterval;
    [Header("刷新数量")]
    [SerializeField]
    private int _updateNum;
    public int UpdateNum => _updateNum;
    [Header("刷新范围(m)")]
    [SerializeField]
    private float _radius;
    public float Radius => _radius;

    [Header("巡逻半径(m)")]
    [SerializeField]
    private float _patrolRadius;
    public float PatrolRadius => _patrolRadius;

    [Header("巡逻速度")]
    [SerializeField]
    private float _patrolSpd;
    public float PatrolSpd => _patrolSpd;

    [Header("巡逻路径")]
    [SerializeField]
    private string _patrolPath = "";
    public string PatrolPath => _patrolPath;
    [Header("AI资源名字")]
    public string AIName;
    public object GetServerData()
    {

        ResourcesPointData data = new()
        {
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            ResourceType = _resourceType,
            ConfigId = _configId,
            UpdateInterval = _updateInterval,
            UpdateNum = _updateNum,
            Radius = _radius,
            PatrolRadius = _patrolRadius,
            PatrolSpd = _patrolSpd,
            PatrolPath = _patrolPath,
            AIName = AIName
        };
        if (!string.IsNullOrEmpty(_levelRange))
        {
            data.LevelRange = DataTableParseUtil.ParseArray<int>(_levelRange);
        }
        return data;
    }
}