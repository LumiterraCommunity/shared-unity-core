/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 路径节点数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/WaypointEventData.cs
 * 
 */

using System;

[Serializable]
public class WaypointEventData
{
    public int Index;
    public string Key;
    public int Duration;
    public string CustomData;
}