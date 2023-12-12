using System.Collections.Generic;
using CMF;
using UnityEngine;

/// <summary>
/// 用来控制角色控制器基础移动的基础移动组件 需要外部给出移动速度 自身有重力控制 业务层一般不直接使用 由CharacterDirectionMove和CharacterDistanceMove来控制移动速度
/// </summary>
public class CharacterMoveCtrl : EntityBaseComponent
{
    private const float AIR_HORIZONTAL_FRICTION = 0.3f;//浮空时水平上的阻力 可以减少翻滚技能等的滑行距离 体验更加友好 每秒减少的比例

    private const float AIR_CONTROL = 6f;//空中下落时的水平移动控制系数 每秒增量的速度

    private Mover _mover;

    /// <summary>
    /// 地面固定移动速度
    /// </summary>
    /// <value></value>
    public Vector3 MoveSpeed { get; private set; }

    /// <summary>
    /// 移动加速度
    /// </summary>
    /// <value></value>
    public Vector3 MoveAccSpeed { get; private set; }

    /// <summary>
    /// 当前移动速度
    /// </summary>
    public Vector3 CurSpeed = Vector3.zero;  //当前移动速度

    /// <summary>
    /// 是否开启重力
    /// </summary>
    public bool EnableGravity = true;

    /// <summary>
    /// 移动速度修改器
    /// </summary>
    private readonly HashSet<MoveModifier> _moveModifiers = new();

    /// <summary>
    /// 在地面上 不是浮空状态
    /// </summary>
    public bool IsGrounded => _mover != null && _mover.IsGrounded();

    private bool _isAddColliderLoadEvent;
    private Sensor.CastType? _specialCastType;//特殊的mover组件检测类型 因为某些时间需要用到特殊的检测类型提高精准度

    private void Start()
    {
        if (!TryGetComponent(out _mover))
        {
            //直接拿不到就要等待加载完成事件
            _isAddColliderLoadEvent = true;
            RefEntity.EntityEvent.ColliderLoadFinish += OnColliderLoadFinish;
        }
        else
        {
            RefreshSettingMover();
        }
        RefEntity.EntityEvent.SetPos += OnSetPosition;
    }

    private void OnDestroy()
    {
        if (_isAddColliderLoadEvent)
        {
            _isAddColliderLoadEvent = false;
            if (RefEntity != null)
            {
                RefEntity.EntityEvent.ColliderLoadFinish -= OnColliderLoadFinish;
            }
        }
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.SetPos -= OnSetPosition;
        }

        RemoveAllMove();
    }

    /// <summary>
    /// 设置特殊的检测类型 可以提高精准度 给null代表取消特殊设置
    /// </summary>
    /// <param name="castType"></param>
    public void SetSpecialCastType(Sensor.CastType? castType)
    {
        _specialCastType = castType;

        if (castType != null)
        {
            RefreshSettingMover();
        }
    }

    private void OnSetPosition(Vector3 pos)
    {
        //位置改变时 激活移动器
        if (!enabled)
        {
            enabled = true;
        }
    }
    private void FixedUpdate()
    {
        if (_mover == null)
        {
            return;
        }

        if (_specialCastType == null)
        {
            _mover.SimpleCheckForGround();
        }
        else
        {
            _mover.CheckForGround();
        }


        bool isGrounded = _mover.IsGrounded();

        if (isGrounded)
        {
            //地面上的移动
            GroundedMovement();
        }
        else
        {
            //加速度计算
            AccMovement();
            //空中移动
            SkyMovement();
        }

        _mover.SetExtendSensorRange(isGrounded);
        // 给移动器正式应用速度
        _mover.SetVelocity(CurSpeed);
        //如果在地面上 且没有移动速度 则返回，避免频繁调用地面检测函数
        if (isGrounded && !IsMove())
        {
            enabled = false;
            return;
        }

    }

    private void AccMovement()
    {
        CurSpeed += MoveAccSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 地表移动
    /// </summary>
    private void GroundedMovement()
    {
        CurSpeed = MoveSpeed;
        //地面上向下的速度不能小于0
        if (CurSpeed.y < 0)
        {
            CurSpeed.y = 0;
        }
    }

    /// <summary>
    /// 空中移动 在空中 会应用重力和空中摩擦力系数, 以及跳跃速度
    /// </summary>
    private void SkyMovement()
    {
        Vector3 velocity = CurSpeed;

        //加重力
        if (EnableGravity)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        //加上水平控制移动速度
        if (MoveDefine.ENABLE_MOVE_IN_AIR)
        {

            Vector3 horizontalVelocity = Vector3.MoveTowards(velocity.OnlyXZ(), MoveSpeed.OnlyXZ(), AIR_CONTROL * Time.deltaTime);
            velocity = horizontalVelocity + velocity.OnlyY();
            //加上水平摩擦力
            velocity -= AIR_HORIZONTAL_FRICTION * Time.deltaTime * velocity.OnlyXZ();
        }
        CurSpeed = velocity;
    }

    /// <summary>
    /// 设置是否启用重力 默认是启用的
    /// </summary>
    /// <param name="enable"></param>
    public void SetEnableGravity(bool enable)
    {
        EnableGravity = enable;
    }

    private void OnColliderLoadFinish(GameObject go)
    {
        _mover = go.GetComponent<Mover>();
        RefreshSettingMover();
    }

    private void RefreshSettingMover()
    {
        if (_mover == null)
        {
            return;
        }

        _mover.SetMoveCtrl(this);

        if (_specialCastType != null)
        {
            _mover.SetSensorCastType(_specialCastType.Value);
        }
    }

    /// <summary>
    /// 添加额外当前移动速度
    /// </summary>
    /// <param name="speed"></param>
    public void AddCurSpeedDelta(Vector3 speed)
    {
        CurSpeed += speed;
    }

    /// <summary>
    /// 设置当前移动速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetCurSpeed(Vector3 speed)
    {
        CurSpeed = speed;
    }

    /// <summary>
    /// 添加移动
    /// </summary>
    /// <param name="speed"></param>
    public MoveModifier AddMove(Vector3 speed, Vector3 accSpeed)
    {

        MoveModifier modifier = MoveModifier.Create(speed, accSpeed);
        _ = _moveModifiers.Add(modifier);
        UpdateSpeed();
        enabled = true;
        return modifier;
    }

    public void RemoveMove(MoveModifier modifier)
    {
        if (_moveModifiers.Remove(modifier))
        {
            modifier.Dispose();
            UpdateSpeed();
        }
    }

    public void UpdateMove(MoveModifier modifier, Vector3 speed, Vector3 accSpeed)
    {
        if (modifier == null)
        {
            return;
        }

        if (modifier.Speed.ApproximatelyEquals(speed) && modifier.AccSpeed.ApproximatelyEquals(accSpeed))
        {
            return;
        }
        modifier.UpdateMove(speed, accSpeed);
        UpdateSpeed();
    }

    public void RemoveAllMove()
    {
        foreach (MoveModifier modifier in _moveModifiers)
        {
            modifier.Dispose();
        }
        _moveModifiers.Clear();
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        MoveSpeed = Vector3.zero;
        MoveAccSpeed = Vector3.zero;
        if (_moveModifiers.Count <= 0)
        {
            return;
        }

        foreach (MoveModifier modifier in _moveModifiers)
        {
            MoveSpeed += modifier.Speed;
            MoveAccSpeed += modifier.AccSpeed;
        }
    }

    /// <summary>
    /// 是否正在移动
    /// </summary>
    /// <returns></returns>
    public bool IsMove()
    {
        return _mover.IsMove || _moveModifiers.Count > 0;
    }
}
