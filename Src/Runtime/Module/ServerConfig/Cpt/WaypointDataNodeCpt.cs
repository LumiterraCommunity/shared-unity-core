/*
 * @Author: xiang huan
 * @Date: 2022-10-17 10:41:46
 * @Description: 自定义事件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/WaypointDataNodeCpt.cs
 * 
 */

using UnityEngine;
using System.Collections.Generic;
public class WaypointDataNodeCpt : MonoBehaviour, IServerDataNodeCpt
{
    [System.Serializable]
    public struct WaypointEvent
    {
        public int Index;
        public eWaypointEventKey Key;
        public int Duration;
        public string CustomData;
    }
    public List<WaypointEvent> Events = new();
    public object GetServerData()
    {
        List<DataNodeBase> pathList = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            DataNodeBase dataNode = new();
            dataNode.X = childTransform.position.x;
            dataNode.Y = childTransform.position.y;
            dataNode.Z = childTransform.position.z;
            pathList.Add(dataNode);
        }
        List<WaypointEventData> events = new();
        for (int i = 0; i < Events.Count; i++)
        {
            WaypointEventData eventData = new();
            eventData.Index = Events[i].Index;
            eventData.Key = Events[i].Key.ToString();
            eventData.Duration = Events[i].Duration;
            eventData.CustomData = Events[i].CustomData;
            events.Add(eventData);
        }

        WaypointPathData config = new();
        config.Key = gameObject.name;
        config.PathList = pathList.ToArray();
        config.Events = events.ToArray();
        return config;
    }
}
