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
    [Header("是否反向移动")]
    public bool ReverseDirection = false;
    [Header("等待时间")]
    public float WaitTime = 1f;
    [Header("移动路径")]
    public List<Transform> Waypoints = new();
    public int CurrentWaypointIndex = 0;
    public Transform CurrentWaypoint;
    private float _waitTime = 0;

    private Rigidbody _rigidBody;
    private TriggerAreaCore _triggerArea;

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
    }

    private void Update()
    {
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
            return;
        }
        MovePlatform();
    }

    private void MovePlatform()
    {

        //If no waypoints have been assigned, return;
        if (Waypoints.Count <= 0)
        {
            return;
        }

        //Calculate a vector to the current waypoint;
        Vector3 _toCurrentWaypoint = CurrentWaypoint.position - transform.position;

        //Get normalized movement direction;
        Vector3 _movement = _toCurrentWaypoint.normalized;

        //Get movement for this frame;
        _movement *= MovementSpeed * Time.deltaTime;

        //If the remaining distance to the next waypoint is smaller than this frame's movement, move directly to next waypoint;
        //Else, move toward next waypoint;
        if (_movement.magnitude >= _toCurrentWaypoint.magnitude || _movement.magnitude == 0f)
        {
            _rigidBody.transform.position = CurrentWaypoint.position;
            UpdateWaypoint();
        }
        else
        {
            _rigidBody.transform.position += _movement;
        }

        if (_triggerArea == null)
        {
            return;
        }

        //Move all controllrs on top of the platform the same distance;

        for (int i = 0; i < _triggerArea.RigidbodiesInTriggerArea.Count; i++)
        {
            if (_triggerArea.RigidbodiesInTriggerArea[i] == null)
            {
                continue;
            }
            _triggerArea.RigidbodiesInTriggerArea[i].MovePosition(_triggerArea.RigidbodiesInTriggerArea[i].position + _movement);
        }
    }

    //This function is called after the current waypoint has been reached;
    //The next waypoint is chosen from the list of waypoints;
    private void UpdateWaypoint()
    {
        if (ReverseDirection)
        {
            CurrentWaypointIndex--;
        }
        else
        {
            CurrentWaypointIndex++;
        }

        //If end of list has been reached, reset index;
        if (CurrentWaypointIndex >= Waypoints.Count)
        {
            CurrentWaypointIndex = 0;
        }

        if (CurrentWaypointIndex < 0)
        {
            CurrentWaypointIndex = Waypoints.Count - 1;
        }

        CurrentWaypoint = Waypoints[CurrentWaypointIndex];
        _waitTime = WaitTime;
    }
}
