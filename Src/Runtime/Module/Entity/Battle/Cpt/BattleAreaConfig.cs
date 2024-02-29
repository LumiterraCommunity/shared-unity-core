/*
 * @Author: xiang huan
 * @Date: 2022-10-09 10:35:21
 * @Description: 战斗区域配置
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/BattleAreaConfig.cs
 * 
 */
using UnityEngine;

/// <summary>
/// 战斗区域配置
/// </summary>
public class BattleAreaConfig : SharedCoreComponent
{
    [Header("区域ID")]
    public int AreaID;

    [Header("优先级")]
    public int Priority = 1;
}