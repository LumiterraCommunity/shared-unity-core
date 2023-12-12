using System;
/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:18:40
 * @Description: 检测带有移动组件的物体
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Scene/TriggerAreaCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using CMF;

public class TriggerAreaCore : SharedCoreComponent
{

    public List<Rigidbody> RigidbodiesInTriggerArea = new();
    public Action<Rigidbody> TriggerEnterRigidbody;
    public Action<Rigidbody> TriggerExitRigidbody;
    private void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody != null && col.GetComponent<Mover>() != null)
        {
            RigidbodiesInTriggerArea.Add(col.attachedRigidbody);
            TriggerEnterRigidbody?.Invoke(col.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.attachedRigidbody != null && col.GetComponent<Mover>() != null)
        {
            _ = RigidbodiesInTriggerArea.Remove(col.attachedRigidbody);
            TriggerExitRigidbody?.Invoke(col.attachedRigidbody);
        }
    }
}
