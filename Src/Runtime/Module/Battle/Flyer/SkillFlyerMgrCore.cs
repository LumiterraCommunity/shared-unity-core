using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 技能飞行物（子弹）管理
/// </summary>
public abstract class SkillFlyerMgrCore<TFlyer> : SceneModuleBase where TFlyer : SkillFlyerCore
{
    private int _nextFlyerID = 1;
    private GameObject _flyerRoot;
    private readonly Dictionary<int, TFlyer> _flyerMap = new();
    /// <summary>
    /// 受管理的飞行物数量
    /// </summary>
    public int FlyerCount => _flyerMap.Count;

    private void Awake()
    {
        _flyerRoot = new GameObject("FlyerRoot");
        _flyerRoot.transform.SetParent(transform, false);
    }

    private void OnDestroy()
    {
        _flyerMap.Clear();

        Destroy(_flyerRoot);
        _flyerRoot = null;
    }

    /// <summary>
    /// 添加一个飞行物 会生成gameObject
    /// </summary>
    /// <param name="formID"></param>
    /// <param name="targetID"></param>
    /// <param name="drSkill"></param>
    /// <returns></returns>
    public TFlyer AddFlyer(long formID, DRSkill drSkill)
    {
        GameObject go = new($"Flyer_{_nextFlyerID}_{drSkill.Id}");
        go.transform.SetParent(_flyerRoot.transform, false);
        TFlyer flyer = go.AddComponent<TFlyer>();
        flyer.Init(_nextFlyerID, formID, drSkill);
        _flyerMap.Add(_nextFlyerID, flyer);
        _nextFlyerID++;

#if UNITY_EDITOR
        go.AddComponent<SkillShapeGizmos>();
#endif
        return flyer;
    }

    /// <summary>
    /// 移除一个飞行物 会销毁gameObject
    /// </summary>
    /// <param name="flyer"></param>
    public void RemoveFlyer(TFlyer flyer)
    {
        if (flyer == null)
        {
            Log.Error($"RemoveFlyer flyer is null");
            return;
        }

        if (!_flyerMap.ContainsKey(flyer.FlyerID))
        {
            Log.Error($"RemoveFlyer flyer not exist {flyer.FlyerID}");
            return;
        }

        _ = _flyerMap.Remove(flyer.FlyerID);
        Destroy(flyer.gameObject);
    }

    /// <summary>
    /// 查询是否有一个飞行物
    /// </summary>
    /// <param name="flyerID"></param>
    /// <returns></returns>
    public bool HasFlyer(int flyerID)
    {
        return _flyerMap.ContainsKey(flyerID);
    }
}