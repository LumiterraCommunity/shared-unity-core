/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 移动平台组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Scene/MovingPlatformCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class MovingPlatformCore : SharedCoreComponent
{

    [Header("移动速度")]
    public float MovementSpeed = 10f;
    [Header("等待时间")]
    public float WaitTime = 1f;
    [Header("移动路径")]
    public List<Transform> Waypoints = new();
    public int CurrentWaypointIndex = 0;
    public Transform CurrentWaypoint;
    private float _waitTime = 0;

    private Rigidbody _rigidBody;
    private TriggerAreaCore _triggerArea;
    private float _allTime = 0;

    private void Start()
    {

        _rigidBody = GetComponent<Rigidbody>();
        _triggerArea = GetComponentInChildren<TriggerAreaCore>();
        _rigidBody.freezeRotation = true;
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = true;

        if (Waypoints.Count <= 0)
        {
            Log.Warning("No waypoints have been assigned to 'MovingPlatform'!");
        }
        else
        {
            CurrentWaypoint = Waypoints[CurrentWaypointIndex];
        }

        CalAllTime();
        UpdatePosition();
    }

    private void CalAllTime()
    {
        _allTime = 0;
        for (int i = 0; i < Waypoints.Count; i++)
        {
            _allTime += WaitTime;
            int targetIndex = i + 1;
            if (i == Waypoints.Count - 1)
            {
                targetIndex = 0;
            }
            _allTime += Vector3.Distance(Waypoints[i].position, Waypoints[targetIndex].position) / MovementSpeed;
        }
    }

    private void UpdatePosition()
    {
        //当前运行时间
        float runTime = TimeUtil.GetServerTimeStamp() / 1000 % (int)_allTime;
        //计算当前时间所在的路径位置
        float tempTime = 0;
        for (int i = 0; i < Waypoints.Count; i++)
        {
            int targetIndex = i + 1;
            if (i == Waypoints.Count - 1)
            {
                targetIndex = 0;
            }
            float moveTime = Vector3.Distance(Waypoints[i].position, Waypoints[targetIndex].position) / MovementSpeed;
            if ((tempTime + moveTime + WaitTime) >= runTime)
            {
                float deltaTime = runTime - tempTime;
                CurrentWaypointIndex = targetIndex;
                CurrentWaypoint = Waypoints[CurrentWaypointIndex];
                transform.position = Waypoints[i].position;
                if (deltaTime > WaitTime)
                {
                    deltaTime -= WaitTime;
                    _waitTime = 0;
                    Vector3 toCurrentWaypoint = CurrentWaypoint.position - transform.position;
                    Vector3 movement = toCurrentWaypoint.normalized;
                    movement *= MovementSpeed * deltaTime;
                    UpdatePositionMovement(movement);
                }
                else
                {
                    _waitTime = WaitTime - deltaTime;
                }
                break;
            }
            else
            {
                tempTime += moveTime + WaitTime;
            }
        }
    }

    private void Update()
    {
        MovePlatform(Time.deltaTime);
    }

    private void MovePlatform(float deltaTime)
    {
        if (Waypoints.Count <= 0)
        {
            return;
        }
        _waitTime -= deltaTime;
        if (_waitTime > 0)
        {
            return;
        }

        Vector3 toCurrentWaypoint = CurrentWaypoint.position - transform.position;
        Vector3 movement = toCurrentWaypoint.normalized;
        movement *= MovementSpeed * deltaTime;

        if (movement.magnitude >= toCurrentWaypoint.magnitude || movement.magnitude == 0f)
        {
            UpdatePosition();
        }
        else
        {
            UpdatePositionMovement(movement);
        }
    }

    private void UpdatePositionMovement(Vector3 movement)
    {
        _rigidBody.transform.position += movement;
        if (_triggerArea == null)
        {
            return;
        }

        for (int i = 0; i < _triggerArea.RigidbodiesInTriggerArea.Count; i++)
        {
            if (_triggerArea.RigidbodiesInTriggerArea[i] == null)
            {
                continue;
            }
            _triggerArea.RigidbodiesInTriggerArea[i].MovePosition(_triggerArea.RigidbodiesInTriggerArea[i].position + movement);
        }
    }
}
