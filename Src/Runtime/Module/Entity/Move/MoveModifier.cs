
/*
* @Author: xiang huan
* @Date: 2023-03-07 19:11:21
* @Description: 移动修改器
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Move/MoveModifier.cs
* 
*/
using GameFramework;
using UnityEngine;

public class MoveModifier : IReference
{

    public Vector3 Speed { get; private set; }
    public Vector3 AccSpeed { get; private set; }  //加速度
    public void Clear()
    {
        Speed = Vector3.zero;
        AccSpeed = Vector3.zero;
    }
    public static MoveModifier Create(Vector3 speed, Vector3 accSpeed)
    {
        MoveModifier modifier = ReferencePool.Acquire<MoveModifier>();
        modifier.Speed = speed;
        modifier.AccSpeed = accSpeed;
        return modifier;
    }

    public void UpdateMove(Vector3 speed, Vector3 accSpeed)
    {
        Speed = speed;
        AccSpeed = accSpeed;

    }
    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }
}