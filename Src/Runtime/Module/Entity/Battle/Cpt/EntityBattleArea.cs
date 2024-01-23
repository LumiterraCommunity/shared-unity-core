/*
 * @Author: xiang huan
 * @Date: 2022-10-09 10:43:29
 * @Description: 区域bgm组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Cpt/EntityBattleArea.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗区域组件
/// </summary>
public class EntityBattleArea : EntityBaseComponent
{
    private readonly List<Collider> _areaQueue = new();
    public eBattleAreaType CurAreaType = eBattleAreaType.Peace;
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

        if (!other.gameObject.TryGetComponent(out BattleAreaConfig _))
        {
            return;
        }

        _areaQueue.Add(other);
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

        if (other == _areaQueue[^1])
        {
            _areaQueue.RemoveAt(_areaQueue.Count - 1);
            UpdateCurAreaConfig();
        }
        else
        {
            _ = _areaQueue.Remove(other);
        }
    }

    /// <summary>
    /// 更新当前区域配置 
    /// </summary>
    private void UpdateCurAreaConfig()
    {
        if (_areaQueue.Count > 0)
        {
            BattleAreaConfig config = _areaQueue[^1].gameObject.GetComponent<BattleAreaConfig>();
            CurAreaType = config.AreaType;
        }
        else
        {
            CurAreaType = eBattleAreaType.Peace;
        }
    }

}