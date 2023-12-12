/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 跳跃平台组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Scene/JumpPlatformCore.cs
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpPlatformCore : SharedCoreComponent
{


    [Header("跳跃速度")]
    public Vector3 JumpSpeed;

    [Header("跳跃延时时间")]
    public float JumpDelayTime = 0.1f;

    [Header("跳跃动画")]
    public Animator JumpAnimator;

    private TriggerAreaCore _triggerArea;
    private float _jumpDelayTime = 0;
    private bool _isJump = false;

    private readonly Dictionary<Rigidbody, MoveModifier> _moveModifiers = new();
    //Start;
    private void Start()
    {
        _triggerArea = GetComponentInChildren<TriggerAreaCore>();
        if (_triggerArea != null)
        {
            _triggerArea.TriggerExitRigidbody += TriggerExitRigidbody;
        }
    }

    private void OnDestroy()
    {
        if (_triggerArea != null)
        {
            _triggerArea.TriggerExitRigidbody -= TriggerExitRigidbody;
        }

        EndTrigger();
    }
    private void Update()
    {
        if (!_isJump)
        {
            if (_triggerArea != null && _triggerArea.RigidbodiesInTriggerArea.Count > 0)
            {
                _jumpDelayTime = JumpDelayTime;
                _isJump = true;
                if (JumpAnimator != null)
                {
                    JumpAnimator.SetBool("IsJump", true);
                }
            }
        }
        else
        {
            if (_jumpDelayTime > 0)
            {
                _jumpDelayTime -= Time.deltaTime;
                return;
            }
            _isJump = false;
            if (JumpAnimator != null)
            {
                JumpAnimator.SetBool("IsJump", false);
            }
            StartTrigger();
        }

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

        MoveModifier modifier = characterMoveCtrl.AddMove(JumpSpeed, Vector3.zero);
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
