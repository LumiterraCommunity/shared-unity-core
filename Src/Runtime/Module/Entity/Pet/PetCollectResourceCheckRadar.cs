using System;
using UnityEngine;

/// <summary>
/// 宠物采集资源检测雷达 挂载在主人身上 检测主人附近的资源
/// </summary>
public class PetCollectResourceCheckRadar : CheckEntityRadarBase
{
    protected override float radius => throw new NotImplementedException();

    protected override LayerMask layer => throw new NotImplementedException();

    /// <summary>
    /// 是否有某种采集物在附近
    /// </summary>
    /// <param name="resType"></param>
    /// <returns></returns>
    public bool IsHaveResourceOnNear(HomeDefine.eAction resType)
    {
        throw new NotImplementedException();

        //TODO: pet
    }
}