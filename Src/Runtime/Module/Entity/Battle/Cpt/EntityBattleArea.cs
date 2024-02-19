/*
 * @Author: xiang huan
 * @Date: 2022-10-09 10:43:29
 * @Description: 战斗区域组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/EntityBattleArea.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 战斗区域组件
/// </summary>
public class EntityBattleArea : EntityBaseComponent
{


    private readonly Dictionary<int, BattleAreaConfig> _areaConfigDic = new();
    public int CurAreaID;
    public eBattleAreaType CurAreaType = eBattleAreaType.Peace;
    public DRBattleArea DRBattleArea { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.EFFECT_TRIGGER)
        {
            return;
        }

        if (!other.gameObject.CompareTag(MTag.BATTLE_AREA))
        {
            return;
        }

        if (!other.gameObject.TryGetComponent(out BattleAreaConfig areaConfig))
        {
            return;
        }
        int code = other.GetHashCode();
        if (_areaConfigDic.ContainsKey(code))
        {
            return;
        }
        _areaConfigDic.Add(code, areaConfig);
        UpdateCurAreaConfig();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != MLayerMask.EFFECT_TRIGGER)
        {
            return;
        }

        if (!other.gameObject.CompareTag(MTag.BATTLE_AREA))
        {
            return;
        }
        if (!other.gameObject.TryGetComponent(out BattleAreaConfig _))
        {
            return;
        }
        int code = other.GetHashCode();
        if (!_areaConfigDic.ContainsKey(code))
        {
            return;
        }
        _ = _areaConfigDic.Remove(code);
        UpdateCurAreaConfig();
    }

    /// <summary>
    /// 更新当前区域配置 
    /// </summary>
    private void UpdateCurAreaConfig()
    {
        if (_areaConfigDic.Count > 0)
        {
            //根据优先级获取当前区域
            int priority = int.MinValue;
            foreach (KeyValuePair<int, BattleAreaConfig> item in _areaConfigDic)
            {
                if (item.Value.Priority > priority)
                {
                    priority = item.Value.Priority;
                    CurAreaID = item.Value.AreaID;
                    DRBattleArea = GFEntryCore.DataTable.GetDataTable<DRBattleArea>().GetDataRow(CurAreaID);
                    if (DRBattleArea != null)
                    {
                        CurAreaType = (eBattleAreaType)DRBattleArea.Type;
                    }
                    else
                    {
                        Log.Error($"UpdateCurAreaConfig id is null {CurAreaID}");
                    }
                }
            }
        }
        else
        {
            CurAreaID = BattleDefine.PEACE_AREA_ID;
            CurAreaType = eBattleAreaType.Peace;
        }
    }

}