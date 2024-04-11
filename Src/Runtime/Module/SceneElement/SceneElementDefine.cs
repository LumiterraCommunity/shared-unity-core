/*
 * @Author: xiang huan
 * @Date: 2024-01-29 11:11:32
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/SceneElementDefine.cs
 * 
 */

/// <summary>
/// 场景元素类型
/// </summary>
public enum eSceneElementType
{
    None = 0,
    SafeArea = 1, //安全区
    Portal = 2, //传送门
    SEArea = 3, //技能效果区域
    SettlePortal = 4, //结算传送门
}
public enum ePortalStatusType
{
    Hide, //隐藏
    Inactive, //未激活
    Activate, //激活状态
    Running, //运行状态
    Finish, //完成状态
}

public enum ePortalType
{
    Exit, //退出传送门
    Area, //区域传送门
}

public enum eSEAreaTriggerType
{
    Enter, //进入触发
    Exit, //退出触发
    Interval, //间隔触发
}