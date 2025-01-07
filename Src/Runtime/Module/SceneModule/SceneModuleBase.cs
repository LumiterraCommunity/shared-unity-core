using UnityEngine;

/// <summary>
/// 场景功能模块基类
/// </summary>
public class SceneModuleBase : MonoBehaviour
{
    /// <summary>
    /// 卸载场景之前
    /// </summary>
    public virtual void UnloadSceneBefore()
    {
        //
    }
    /// <summary>
    /// 卸载场景之后
    /// </summary>
    public virtual void UnloadSceneAfter()
    {
        //
    }

    /// <summary>
    /// 外部时序控制
    /// </summary>
    public void EnterScene()
    {
        OnEnterScene();
    }

    /// <summary>
    /// 外部时序控制
    /// </summary>
    public void ReconnectionScene()
    {
        OnReconnectionScene();
    }


    /// <summary>
    /// 进入场景，这个时序已经可以和服务器通讯
    /// </summary>
    protected virtual void OnEnterScene()
    {
        //
    }


    /// <summary>
    /// 重连进入场景
    /// </summary>
    protected virtual void OnReconnectionScene()
    {
        //
    }
}