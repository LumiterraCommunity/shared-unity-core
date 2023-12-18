using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;

/// <summary>
/// 玩家进出区域记录
/// </summary>
public class PlayerAreaRecord : EntityBaseComponent
{
    public long PlayerID { get; private set; }
    /// <summary>
    /// 当前区域堆栈
    /// 为什么不用栈？因为存在移除中间区域的可能，所以不能用栈，只能用List
    /// 为什么不用链表？因为数量级不会很大，List的长度撑死也就十几个，所以用List，用起来方便
    /// </summary>
    /// <returns></returns>
    private readonly List<SceneAreaInfo> _areaList = new();
#if UNITY_EDITOR
    /// <summary>
    /// 用于编辑器显示当前区域堆栈
    /// 为空标识在默认区域（大世界）
    /// </summary>
    /// <returns></returns>
    public List<eSceneArea> AreaStack = new();
#endif
    /// <summary>
    /// 进入区域处理队列
    /// </summary>
    /// <returns></returns>
    private readonly List<SceneAreaInfo> _readyToEnterList = new();
    /// <summary>
    /// 退出区域处理队列
    /// </summary>
    /// <returns></returns>
    private readonly List<SceneAreaInfo> _readyToExitList = new();
    /// <summary>
    /// player当前所在区域，默认值为大世界
    /// </summary>
    /// <value></value>
    [SerializeField]
    private eSceneArea _curArea = eSceneArea.World;
    public eSceneArea CurArea { get => _curArea; private set => _curArea = value; }

    private void Awake()
    {
        PlayerID = RefEntity.BaseData.Id;
    }

    /// <summary>
    /// 接受区域变化事件，进入区域和离开区域都会触发
    /// 先存入队列，等帧结束后再进行通过ApplyAreaChangedEvent处理
    /// </summary>
    /// <param name="info"></param>
    public void ReceiveAreaChangedEvent(SceneAreaInfo record, eAreaChangedType type)
    {
        if (type == eAreaChangedType.enter)
        {
            PushToPendingList(record, _readyToEnterList);
        }
        else if (type == eAreaChangedType.exit)
        {
            PushToPendingList(record, _readyToExitList);
        }
    }

    /// <summary>
    /// 区域变更待处理队列
    /// 因为有多个区域同时进入的可能，所以需要按照优先级进行排序，等帧结束后再处理这些区域变化
    /// </summary>
    /// <param name="info"></param>
    private void PushToPendingList(SceneAreaInfo info, List<SceneAreaInfo> targetList)
    {
        int insertIndex = 0;
        for (; insertIndex < targetList.Count; insertIndex++)
        {
            if (info.Priority > targetList[insertIndex].Priority)
            {
                break;
            }
        }
        targetList.Insert(insertIndex, info);
    }

    /// <summary>
    /// 进入区域处理，会立刻改变当前区域
    /// 如果想要延迟处理，可以使用ReceiveAreaChangedEvent(info, eAreaChangedType.enter)
    /// </summary>
    /// <param name="info"></param>
    public void EnterAreaTrigger(SceneAreaInfo info)
    {
        // 正常情况下不会触发，但测试的时候发现，如果把gameObject的active设置为false，然后再设置为true就会触发
        // 原因是把gameObject的active设置为false，不会触发OnTriggerExit，区域数据不会清空，但是把gameObject的active设置为true，会触发OnTriggerEnter,又把区域数据放进去了，导致重复添加区域数据
        if (CheckIsRepeatEnterArea(info))
        {
            Log.Error($"repeat enter area:{info}");
            return;
        }

        _areaList.Add(info);
        CurArea = info.Area;
#if UNITY_EDITOR
        AreaStack.Add(info.Area);
#endif
    }

    /// <summary>
    /// 退出区域触发器，会立刻改变当前区域
    /// 如果想要延迟处理，可以使用ReceiveAreaChangedEvent(area, eAreaChangedType.exit)
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public void ExitAreaTrigger(SceneAreaInfo area)
    {
        if (_areaList.Count == 0)
        {
            Log.Error("ExitArea error, areaQueue is empty");
            return;
        }

        for (int i = _areaList.Count - 1; i >= 0; i--)
        {
            if (_areaList[i].AreaID == area.AreaID)
            {
                _areaList.RemoveAt(i);
#if UNITY_EDITOR
                AreaStack.RemoveAt(i);
#endif
                break;
            }
        }

        CurArea = _areaList.Count == 0 ? eSceneArea.World : _areaList[^1].Area;
    }

    /// <summary>
    /// 应用区域改变
    /// </summary>
    private void ApplyAreaChanged()
    {
        foreach (SceneAreaInfo info in _readyToEnterList)
        {
            EnterAreaTrigger(info);
        }

        foreach (SceneAreaInfo info in _readyToExitList)
        {
            ExitAreaTrigger(info);
        }

        _readyToEnterList.Clear();
        _readyToExitList.Clear();
    }

    /// <summary>
    /// 检查是否重复进入同一个区域
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private bool CheckIsRepeatEnterArea(SceneAreaInfo info)
    {
        //_areaList数量级不会很大，撑死就十来个，也不是每帧都会进入，所以直接遍历检查，没必要再用一个专门查重的字典
        for (int i = 0; i < _areaList.Count; i++)
        {
            SceneAreaInfo areaInfo = _areaList[i];
            if (areaInfo.AreaID == info.AreaID)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        eSceneArea lastArea = CurArea;
        ApplyAreaChanged();
        if (lastArea != CurArea)
        {
            Log.Info($"PlayerEnterAreaInfo, playerID:{PlayerID}, curArea:{CurArea}, lastArea:{lastArea}");
            GFEntryCore.GetModule<SceneAreaMgrCore>().OnPlayerExitCurSceneCheckArea?.Invoke(PlayerID, lastArea);//离开当前区域事件
            GFEntryCore.GetModule<SceneAreaMgrCore>().OnPlayerEnterNewSceneCheckArea?.Invoke(PlayerID, CurArea);//进入新区域事件
        }
    }
}