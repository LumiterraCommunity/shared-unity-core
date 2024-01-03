/*
 * @Author: xiang huan
 * @Date: 2023-04-21 10:58:10
 * @Description: 实体网络数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/EntityBaseNetDataCore.cs
 * 
 */
using GameMessageCore;
using UnityGameFramework.Runtime;

public class EntityBaseNetDataCore : EntityBaseComponent
{
    protected EntityWithLocation EntityWithLocation;
    private bool _isInit = false;
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        if (_isInit)
        {
            return;
        }
        _isInit = true;
    }

    /// <summary>
    /// 创建数据
    /// </summary>
    public virtual void CreateData()
    {
        EntityWithLocation = new();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public virtual void UpdateData()
    {

    }

    /// <summary>
    /// 获取数据
    /// </summary>
    public virtual EntityWithLocation GetData()
    {
        if (!_isInit)
        {
            Init();
        }

        try
        {
            if (EntityWithLocation == null || EntityWithLocation.IsLock)
            {
                CreateData();
            }
            UpdateData();
            EntityWithLocation.IsLock = true;
        }
        catch (System.Exception e)
        {
            Log.Error($"EntityBaseNetDataCore GetData Error:{e}");
            if (EntityWithLocation == null)
            {
                EntityWithLocation = new();
            }
        }
        return EntityWithLocation;
    }

}