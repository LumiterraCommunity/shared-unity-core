using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 畜牧食盆
/// </summary>
public class AnimalBowlCore : MonoBehaviour, ICollectResourceCore
{
    public AnimalBowlData Data { get; private set; }

    public ulong Id => Data.SaveData.BowlId;

    public eResourceType ResourceType => eResourceType.AnimalBowl;

    public GameObject LogicRoot => gameObject;

    public Vector3 Position => transform.position;

    public eAction SupportAction => eAction.PutAnimalFood;

    public float Lv
    {
        get
        {
            Log.Error("AnimalBowlCore.Lv is not implemented");
            return 0;
        }
    }

    private void Awake()
    {
        Data = gameObject.AddComponent<AnimalBowlData>();
    }

    protected virtual void OnDestroy()
    {
        Destroy(Data);
    }

    /// <summary>
    /// 食盆被消耗掉食物 设置剩余容量
    /// </summary>
    public virtual void CostSetCapacity(int remainCapacity)
    {
        Data.SaveData.RemainFoodCapacity = remainCapacity;
        if (Data.SaveData.RemainFoodCapacity <= 0)
        {
            Data.UpdateFood(0, 0);
        }
    }

    public bool CheckSupportAction(eAction action)
    {
        return (SupportAction & action) != 0 && !Data.IsHaveFood;
    }

    public bool CheckPlayerAction(long playerId, eAction action)
    {
        if (!HomeModuleCore.HomeData.IsPersonalHome)
        {
            return false;
        }

        return CheckSupportAction(action);
    }

    public virtual void ExecuteAction(eAction action, int toolCid, long playerId, long entityId, int skillId, object actionData)
    {
        DRAnimalFood drAnimalFood = GFEntryCore.DataTable.GetDataTable<DRAnimalFood>().GetDataRow(toolCid);
        if (drAnimalFood == null)
        {
            throw new Exception($"食盆执行动作时食物配置表里没有找到cid为 {toolCid} 的食物");
        }

        Data.UpdateFood(toolCid, drAnimalFood.Capacity);
    }

    public void ExecuteProgress(eAction targetCurAction, long triggerEntityId, int skillId, int deltaProgress, bool isCrit, bool isPreEffect)
    {

    }
}