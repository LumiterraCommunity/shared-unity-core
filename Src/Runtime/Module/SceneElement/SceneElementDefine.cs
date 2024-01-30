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
}

public enum ePortalStatusType
{
    Inactive = 0,     //未开启
    Open = 1,         //开启
    Running = 3,      //运行中
    Finish = 2,       //完成
}