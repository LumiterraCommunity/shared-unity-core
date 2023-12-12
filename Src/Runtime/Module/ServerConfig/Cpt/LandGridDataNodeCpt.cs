/*
 * @Author: xiang huan
 * @Date: 2022-06-27 14:13:48
 * @Description: 领地格子数据组件
 * @FilePath: /Assets/Src/Module/ServerScene/Cpt/LandGridDataNodeCpt.cs
 * 
 */
using UnityEngine;
public class LandGridDataNodeCpt : MonoBehaviour, IServerDataNodeCpt
{
    public int ID;
    public object GetServerData()
    {
        LandGridData data = new();
        data.ID = ID;
        data.X = transform.position.x;
        data.Y = transform.position.y;
        data.Z = transform.position.z;
        return data;
    }
}