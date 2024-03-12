using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 家园实体基类core 是挂载土地根节点上的一个脚本 不是走的EntityBase的实体 是属于土地上的特殊种子的功能实体 和土地有一定耦合关系 更加轻巧且状态同步更加简单
/// </summary>
public abstract class HomeEntityCore : MonoBehaviour
{
    /// <summary>
    /// 家园实体的唯一id 目前和土地id一样
    /// </summary>
    /// <value></value>
    public long Id { get; private set; }
    /// <summary>
    /// 实体类型
    /// </summary>
    /// <value></value>
    public SeedFunctionType Type { get; private set; }

    /// <summary>
    /// 关联引用的土地
    /// </summary>
    /// <value></value>
    public HomeSoilCore RefSoil { get; private set; }
    /// <summary>
    /// 实体自身的消息事件
    /// </summary>
    /// <value></value>
    public HomeEntityEventCore EntityEvent { get; protected set; }

    /// <summary>
    /// 初始化工作
    /// </summary>
    /// <param name="soil"></param>
    internal void Init(HomeSoilCore soil)
    {
        RefSoil = soil;

        OnInit();

        IHomeEntityInitLogic[] initLogics = GetComponents<IHomeEntityInitLogic>();
        if (initLogics == null || initLogics.Length == 0)
        {
            Log.Error("家园实体 没有找到初始化逻辑");
            return;
        }

        foreach (IHomeEntityInitLogic logic in initLogics)
        {
            try
            {
                logic.LogicInit(this);
            }
            catch (System.Exception e)
            {
                Log.Error($"家园实体初始化逻辑出错 type:{Type} {logic.GetType().Name} error:{e}");
                continue;
            }
        }

        EntityEvent.OnInitFinished?.Invoke();
    }

    internal void Dispose()
    {
        try
        {
            EntityEvent.OnEntityRemoved?.Invoke();

            OnDispose();
        }
        catch (System.Exception e)
        {
            Log.Error($"家园实体释放逻辑出错 type:{Type} error:{e}");
        }

        SetBaseInfo(0, SeedFunctionType.None);
        Destroy(gameObject);
    }

    internal void SetBaseInfo(long id, SeedFunctionType type)
    {
        Id = id;
        Type = type;
    }

    /// <summary>
    /// 子类初始化 这是实体上的通用初始化 不同实体业务上的初始化添加HomeEntityInitLogic子类来实现
    /// </summary>
    protected abstract void OnInit();
    /// <summary>
    /// 子类释放
    /// </summary>
    protected abstract void OnDispose();
}