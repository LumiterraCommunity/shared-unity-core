/*
 * @Author: XH
 * @Date: 2023-10-24 15:14:29
 * @Description: 塔关卡
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/TowerLevelElementCore.cs
 * 
 */

using System.Collections.Generic;
using UnityEngine;

public class TowerLevelElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.TowerLevel;
    [Header("出生点")]
    public Vector3 BirthPoint;

    [Header("刷新区域")]
    public List<HomeResourcesArea> HomeResourcesAreaList;
}
