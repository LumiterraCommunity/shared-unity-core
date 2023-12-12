
/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 风平台组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Scene/WindPlatformCore.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;

public class WindPlatformCore : SharedCoreComponent
{
    [Header("风力方向")]
    public Vector3 WindSpeed;

    [Header("是否开启重力")]
    public bool EnableGravity = true;
    private TriggerAreaCore _triggerArea;

    private readonly Dictionary<Rigidbody, MoveModifier> _moveModifiers = new();

    private void Start()
    {
        _triggerArea = GetComponentInChildren<TriggerAreaCore>();
        if (_triggerArea != null)
        {
            _triggerArea.TriggerEnterRigidbody += TriggerEnterRigidbody;
            _triggerArea.TriggerExitRigidbody += TriggerExitRigidbody;
            StartTrigger();
        }
    }

    private void OnDestroy()
    {
        if (_triggerArea != null)
        {
            _triggerArea.TriggerEnterRigidbody -= TriggerEnterRigidbody;
            _triggerArea.TriggerExitRigidbody -= TriggerExitRigidbody;
        }

        EndTrigger();
    }
    private void StartTrigger()
    {
        foreach (Rigidbody rigidbody in _triggerArea.RigidbodiesInTriggerArea)
        {
            TriggerEnterRigidbody(rigidbody);
        }
    }

    private void EndTrigger()
    {
        List<Rigidbody> rigidbodys = new(_moveModifiers.Keys);
        for (int i = 0; i < rigidbodys.Count; i++)
        {
            TriggerExitRigidbody(rigidbodys[i]);
        }
        _moveModifiers.Clear();
    }
    private void TriggerEnterRigidbody(Rigidbody rigidbody)
    {
        if (rigidbody == null)
        {
            return;
        }

        if (!rigidbody.gameObject.TryGetComponent(out CharacterMoveCtrl characterMoveCtrl))
        {
            return;
        }

        if (_moveModifiers.ContainsKey(rigidbody))
        {
            return;
        }

        Vector3 acc = EnableGravity ? Vector3.zero : -Physics.gravity;
        //风力移动设置
        MoveModifier modifier = characterMoveCtrl.AddMove(WindSpeed, acc);
        _moveModifiers.Add(rigidbody, modifier);
    }

    private void TriggerExitRigidbody(Rigidbody rigidbody)
    {
        if (!rigidbody.gameObject.TryGetComponent(out CharacterMoveCtrl characterMoveCtrl))
        {
            return;
        }

        if (_moveModifiers.TryGetValue(rigidbody, out MoveModifier moveModifier))
        {
            characterMoveCtrl.RemoveMove(moveModifier);
            _ = _moveModifiers.Remove(rigidbody);
        }
    }


}
