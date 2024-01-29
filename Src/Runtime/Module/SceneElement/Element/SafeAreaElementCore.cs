/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 安全区组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/SafeAreaElementCore.cs
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class SafeAreaElementCore : SceneElementCore
{
    public struct SafeAreaInfo
    {
        public float Radius;
        public float Speed;
        public float WaitTime;
        public float Damage;
    }
    public override eSceneElementType ElementType => eSceneElementType.SafeArea;

    [Header("是否运行")]
    public bool IsRun = false;

    [Header("当前半径")]
    public float CurRadius = 0;

    [Header("初始半径")]
    public float InitRadius = 0;

    [Header("安全列表")]
    public List<SafeAreaInfo> SafeAreaInfos;
    private int _curIndex = 0;
    private float _waitTime = 0;
    private void Start()
    {
        CurRadius = InitRadius;
        SetCurIndex(0);
    }
    public void Run(float runTime = 0)
    {
        IsRun = true;
        UpdateSafeArea(runTime);
    }
    public void Stop()
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
        if (safeZoneInfo.Radius <= CurRadius)
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
        //只计算x和z距离
        return Vector3.Distance(pos.OnlyXZ(), transform.position.OnlyXZ()) <= CurRadius;
    }

    public override void InitNetData(string netData)
    {
        if (string.IsNullOrEmpty(netData))
        {
            return;
        }
        SafeAreaElementNetData config = JsonConvert.DeserializeObject<SafeAreaElementNetData>(netData);
        transform.position = config.Position;
    }
    public override string GetNetData()
    {
        SafeAreaElementNetData netData = new()
        {
            Position = transform.position
        };
        return netData.ToJson();
    }
}
