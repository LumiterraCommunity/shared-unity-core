/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 副本关卡数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/InstancingLevelData.cs
 * 
 */
using System;
using System.Numerics;

[Serializable]
public class InstancingLevelData : DataNodeBase
{
    public string LevelAI;
    public ResourcesPointData[] ResourcesPointList;
    public Vector3[] BirthPointList;
}