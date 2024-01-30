/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 传送门组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/PortalElementCore.cs
 * 
 */
using UnityEngine;
using Newtonsoft.Json;

public class PortalElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.Portal;

    [Header("传送门状态")]
    public ePortalStatusType StatusType = ePortalStatusType.Inactive;

    [Header("当前使用次数")]
    public int CurUseNum = 0;

    [Header("最大使用次数")]
    public int MaxUseNum = 0;
    private long _startTime = 0;

    protected override void Update()
    {
        base.Update();
        UpdateStatusInactive();
    }
    public override void UpdateElementData()
    {
        base.UpdateElementData();
        PortalElementNetData netData = new()
        {
            StartTime = _startTime,
            StatusType = StatusType,
            CurUseNum = CurUseNum,
        };
        SceneElementData.ElementData = netData.ToJson();
    }

    public void StartElement(long startTime, ePortalStatusType statusType, int curUseNum)
    {
        _startTime = startTime;
        StatusType = statusType;
        CurUseNum = curUseNum;
        UpdateElementData();
    }

    public override void InitElementData(GameMessageCore.SceneElementData netData)
    {
        PortalElementNetData config = JsonConvert.DeserializeObject<PortalElementNetData>(netData.ElementData);
        StartElement(config.StartTime, config.StatusType, config.CurUseNum);
    }

    private void UpdateStatusInactive()
    {
        if (StatusType != ePortalStatusType.Inactive)
        {
            return;
        }
        long curTime = TimeUtil.GetServerTimeStamp();
        if (curTime < _startTime)
        {
            return;
        }
        StatusType = ePortalStatusType.Open;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }
        EntityBase entity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(other.gameObject);
        if (entity == null || !entity.Inited || !entity.BattleDataCore.IsLive())
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.PLAYER)
        {
            return;
        }

    }

}
