/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 召唤资源点数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/SummonResourcesPointData.cs
 * 
 */
using System;

[Serializable]
public class SummonResourcesPointData : ResourcesPointData
{
    public long FromID;   //召唤者ID
}