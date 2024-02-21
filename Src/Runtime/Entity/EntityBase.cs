using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 场景实体,前后端可以直接使用，或者根据差异化派生出新的类
/// </summary>
public class EntityBase
{
    /// <summary>
    /// 实体是否初始化，dispose后变为false
    /// </summary>
    /// <value></value>
    public bool Inited { get; protected set; } = false;
    /// <summary>
    /// 逻辑实体根节点 可以挂载逻辑实体相关逻辑 一定不为空
    /// </summary>
    /// <value></value>
    protected GameObject Root;
    /// <summary>
    /// 谨慎使用，不允许往上添加组件和直接修改Transform
    /// </summary>
    /// <value></value>
    public GameObject EntityRoot => Root;
    /// <summary>
    /// 场景逻辑节点的名字
    /// </summary>
    public string Name => Root.name;
    /// <summary>
    /// 逻辑实体的root节点的id，用于通过root节点id来获取逻辑实体
    /// </summary>
    /// <returns></returns>
    public int RootID => Root.GetInstanceID();
    /// <summary>
    /// 场景实体变换 只能给 也是Root节点的变换  不能使用上面的坐标等属性 也不能赋值  有统一的方法
    /// </summary>
    private Transform _transform;
    public Vector3 Position => _transform.position;
    /// <summary>
    /// 正方向向量
    /// </summary>
    public Vector3 Forward => _transform.forward;
    /// <summary>
    /// 右方向向量
    /// </summary>
    public Vector3 Right => _transform.right;
    /// <summary>
    /// 欧拉角 里面都是弧度
    /// </summary>
    public Vector3 EulerAngles => _transform.eulerAngles;
    /// <summary>
    /// 最常见的本地缩放值
    /// </summary>
    public Vector3 Scale => _transform.localScale;
    public Quaternion Rotation => _transform.rotation;

    /// <summary>
    /// 加速获取 缓存了 entity最基础的数据组件引用
    /// </summary>
    public readonly EntityBaseData BaseData;

    /// <summary>
    /// 加速获取 缓存了 enitty 内部事件的组件 子类构建时就需要添加好对应组件
    /// </summary>
    public EntityEvent EntityEvent { get; protected set; }

    /// <summary>
    /// 移动相关数据 由于服务器频繁查询 放到基类存放
    /// </summary>
    /// <value></value>
    public EntityMoveData MoveData { get; set; }

    /// <summary>
    /// 战斗数据
    /// </summary>
    /// <value></value>
    public EntityBattleDataCore BattleDataCore { get; set; }

    /// <summary>
    /// 捕获数据
    /// </summary>
    /// <value></value>
    public EntityCaptureDataCore CaptureData { get; set; }

    /// <summary>
    /// 角色的基础数据 名字 宽高等
    /// </summary>
    /// <value></value>
    public RoleBaseDataCore RoleBaseDataCore { get; set; }

    /// <summary>
    /// 整个root gameObject的激活状态 一般不用乱用
    /// </summary>
    /// <value></value>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// 实体属性数据
    /// </summary>
    /// <value></value>
    public EntityAttributeData EntityAttributeData { get; set; }

    /// <summary>
    /// 网络数据
    /// </summary>
    /// <value></value>
    public EntityBaseNetDataCore NetData { get; set; }

    /// <summary>
    /// 网络数据
    /// </summary>
    /// <value></value>
    public EntityCheckPosCore EntityCheckPosCore { get; set; }

    /// <summary>
    /// 不要乱用 读写相关属性都有独立方法 只能在特定情境下只能通过获取Transform来获取时使用
    /// </summary>
    /// <returns></returns>
    public Transform GetTransform()
    {
        return _transform;
    }

    public EntityBase()
    {
        InitRoot();
        BaseData = RootAddComponent<EntityBaseData>();
    }

    public virtual void Dispose()
    {
        try
        {
            EntityEvent.UnInitFromScene?.Invoke(this);
            Inited = false;
            UnInitFromScene();
            BaseData.Reset();
        }
        catch (System.Exception e)
        {
            Log.Error($"EntityBase Dispose error = {e}");
        }

        Object.Destroy(Root);
    }

    public virtual void Init()
    {
        Inited = true;
        InitToScene();
        EntityEvent.InitToScene?.Invoke(this);
    }

    protected virtual void InitRoot()
    {
        SetRoot(new GameObject());
    }

    protected void SetRoot(GameObject root)
    {
        Root = root;
        _transform = Root.transform;
    }

    /// <summary>
    /// 添加到场景中
    /// </summary>
    protected virtual void InitToScene()
    {
        //todo:
    }

    protected virtual void UnInitFromScene()
    {
        //todo;
    }

    /// <summary>
    /// 统一的 设置位置的地方 除了一些基础组件自己控制了位置 其他都用这个方法
    /// </summary>
    /// <param name="pos"></param>
    public virtual void SetPosition(Vector3 pos)
    {
        _transform.position = pos;
        EntityEvent.SetPos?.Invoke(pos);
    }

    /// <summary>
    /// 统一的 设置正方向朝向的地方 上层需要使方向水平 目前实体水平上不旋转
    /// </summary>
    /// <param name="forward"></param>
    public virtual void SetForward(Vector3 forward)
    {
        //限制y轴不变 都是水平的 目前看起来实体这样也才更加合理 为什么要打印是因为业务层没有考虑到0说明其他地方也可能会有问题
        if (!forward.y.ApproximatelyEquals(0))
        {
            forward.y = 0;
        }
        _transform.forward = forward;
    }

    public void InitBaseInfo(long id, GameMessageCore.EntityType type)
    {
        BaseData.Init(id, type);
        Root.name = $"{type}_{id}";
    }
    public string GetSkillEffectData()
    {
        if (TryGetComponent(out SkillEffectCpt skillEffectCpt))
        {
            return skillEffectCpt.GetNetData();
        }
        return "";
    }

    public void SetRootName(string name)
    {
        Root.name = name;
    }

    public void SetRootParent(Transform parent)
    {
        _transform.SetParent(parent, false);
    }


    /// <summary>
    /// 设置整个root gameObject的激活状态 一般不用乱用
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        if (IsActive == active)
        {
            return;
        }

        IsActive = active;
        Root.SetActive(active);
    }

    public T GetComponent<T>()
    {
        return Root.GetComponent<T>();
    }

    public T AddComponent<T>() where T : Component
    {
        return RootAddComponent<T>();
    }

    public bool TryGetComponent<T>(out T component)
    {
        return Root.TryGetComponent(out component);
    }

    public bool HasComponent<T>()
    {
        return Root.TryGetComponent(out T t);
    }

    /// <summary>
    /// 慎用 会导致维护性下降 不能明确知道添加组件的位置 获取组件 如果没有会添加一个保证一定有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddComponent<T>() where T : Component
    {
        if (Root.TryGetComponent(out T t))
        {
            return t;
        }

        return RootAddComponent<T>();
    }

    protected T RootAddComponent<T>() where T : Component
    {
        T t = Root.AddComponent<T>();
        if (t is IEntityComponent entityComponent)
        {
            entityComponent.InitEntity(this);
        }
        return t;
    }

    /// <summary>
    ///  获取所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetComponents<T>()
    {
        return Root.GetComponents<T>();
    }
}