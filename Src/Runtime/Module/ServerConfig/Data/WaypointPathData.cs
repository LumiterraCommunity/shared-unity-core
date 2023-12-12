/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 路径节点数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/WaypointPathData.cs
 * 
 */
using System;

[Serializable]
public class WaypointPathData
{
    public string Key;
    public DataNodeBase[] PathList;
    public WaypointEventData[] Events;
}