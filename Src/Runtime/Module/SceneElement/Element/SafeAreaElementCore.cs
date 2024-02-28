/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 安全区组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/SafeAreaElementCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityGameFramework.Runtime;

public class SafeAreaElementCore : SceneElementCore
{

    public override eSceneElementType ElementType => eSceneElementType.SafeArea;

    [Header("是否运行")]
    public bool IsRun = false;

    [Header("当前半径")]
    public float CurRadius = 0;

    [Header("初始半径")]
    public float InitRadius = 0;
    [Serializable]
    public struct SafeAreaInfo
    {
        public float Radius;
        public float Speed;
        public float WaitTime;
        public float Damage;
    }
    [Header("安全列表")]
    public List<SafeAreaInfo> SafeAreaInfos;
    [Header("安全区特效")]
    public GameObject SafeAreaEffect;
    private int _curIndex = 0;
    private float _waitTime = 0;
    private long _startTime = 0;
    private void Start()
    {
        CurRadius = InitRadius;
        SetCurIndex(0);
    }
    protected void Run(float runTime)
    {
        IsRun = true;
        UpdateSafeArea(runTime);
    }
    protected void Stop()
    {
        IsRun = false;
    }
    protected override void Update()
    {
        base.Update();
        if (!IsRun)
        {
            return;
        }
        UpdateSafeArea(Time.deltaTime);
    }

    public void UpdateSafeArea(float deltaTime)
    {
        if (SafeAreaInfos.Count == 0)
        {
            return;
        }
        bool isEnd = (_curIndex == SafeAreaInfos.Count - 1) && _waitTime <= 0;
        while (deltaTime > 0 && !isEnd)
        {
            deltaTime = UpdateMove(deltaTime);
            deltaTime = UpdateWait(deltaTime);
            if (_waitTime <= 0)
            {
                SetCurIndex(_curIndex + 1);
            }
        }
    }

    private float UpdateMove(float deltaTime)
    {
        if (deltaTime <= 0)
        {
            return deltaTime;
        }
        SafeAreaInfo safeZoneInfo = SafeAreaInfos[_curIndex];
        if (CurRadius <= safeZoneInfo.Radius)
        {
            return deltaTime;
        }

        float moveTime = Math.Max(CurRadius - safeZoneInfo.Radius, 0) / safeZoneInfo.Speed;
        if (moveTime > deltaTime)
        {
            CurRadius -= safeZoneInfo.Speed * deltaTime;
            deltaTime = 0;
        }
        else
        {
            CurRadius = safeZoneInfo.Radius;
            deltaTime -= moveTime;
        }
        if (SafeAreaEffect != null)
        {
            SafeAreaEffect.transform.localScale = 2 * CurRadius * Vector3.one;
        }
        return deltaTime;
    }

    private float UpdateWait(float deltaTime)
    {
        if (deltaTime <= 0)
        {
            return deltaTime;
        }
        if (_waitTime <= 0)
        {
            return deltaTime;
        }

        if (_waitTime > deltaTime)
        {
            _waitTime -= deltaTime;
            deltaTime = 0;
        }
        else
        {
            _waitTime = 0;
            deltaTime -= _waitTime;
        }
        return deltaTime;
    }

    private void SetCurIndex(int index)
    {
        if (index >= 0 && index < SafeAreaInfos.Count)
        {
            SafeAreaInfo safeZoneInfo = SafeAreaInfos[index];
            _waitTime = safeZoneInfo.WaitTime;
            _curIndex = index;
        }
    }

    public SafeAreaInfo GetCurSafeAreaInfo()
    {
        return SafeAreaInfos[_curIndex];
    }

    public bool IsSafeArea(Vector3 pos)
    {
        return Vector3.Distance(pos, transform.position) <= CurRadius;
    }

    public override void UpdateElementData()
    {
        base.UpdateElementData();
        GameMessageCore.SafeAreaElementData netData = new()
        {
            StartTime = _startTime,
        };
        netData.Position.Set(transform.position.x, transform.position.y, transform.position.z);
        SceneElementData.SafeArea = netData;
    }

    public void StartElement(Vector3 pos, long startTime)
    {
        _startTime = startTime;
        transform.position = pos;
        float runTime = (TimeUtil.GetServerTimeStamp() - _startTime) * TimeUtil.MS2S;
        Run(runTime);
        UpdateElementData();
    }

    public override void InitElementData(GameMessageCore.SceneElementData netData)
    {
        GameMessageCore.SafeAreaElementData safeArea = netData.SafeArea;
        StartElement(new Vector3(safeArea.Position.X, safeArea.Position.Y, safeArea.Position.Z), safeArea.StartTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CurRadius);
    }
}
