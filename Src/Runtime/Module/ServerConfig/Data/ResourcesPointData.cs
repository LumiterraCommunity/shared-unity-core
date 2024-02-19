/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 资源点数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/ResourcesPointData.cs
 * 
 */
using System;

[Serializable]
public class ResourcesPointData : DataNodeBase
{
    public int ResourceType;
    public int ConfigId;
    public int[] LevelRange;
    public int UpdateInterval;
    public int UpdateNum;
    public float Radius;
    public float PatrolRadius;
    public float PatrolSpd;
    public string PatrolPath;
    public string AIName;
    public int Lv;//等级
}