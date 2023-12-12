/*
 * @Author: xiang huan
 * @Date: 2022-06-27 16:25:51
 * @Description: 领地格子数据
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Data/LandGridData.cs
 * 
 */
using System;

public class LandGridData : DataNodeBase, IComparable<LandGridData>
{
    public int ID;

    public int CompareTo(LandGridData other)
    {
        return ID - other.ID;
    }
}