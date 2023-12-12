using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class AnimalInitLogicCore : MonoBehaviour
{
    /// <summary>
    /// 恢复畜牧食盆数据
    /// </summary>
    /// <param name="bowlDataArr">可给null 代表没有存档数据</param>
    public void RestoreBowlData<T>(AnimalBowlSaveData[] bowlDataArr) where T : AnimalBowlCore
    {
        List<T> bowlList = RestoreBowlEntityList<T>(bowlDataArr);

        //按照食盆id排序 吃的顺序是最终数组顺序
        bowlList.Sort((a, b) => a.Data.SaveData.BowlId < b.Data.SaveData.BowlId ? -1 : 1);

        HomeAnimalScene homeAnimalScene = GetComponent<HomeAnimalScene>();
        for (int i = 0; i < bowlList.Count; i++)
        {
            homeAnimalScene.AddBowls(bowlList[i]);
        }
    }

    /// <summary>
    /// 获取恢复出来的食盆实体列表 不返回null
    /// </summary>
    /// <param name="bowlDataArr">null代表没有存档数据</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static List<T> RestoreBowlEntityList<T>(AnimalBowlSaveData[] bowlDataArr) where T : AnimalBowlCore
    {
        GameObject[] bowlGos = GameObject.FindGameObjectsWithTag(MTag.HOME_ANIMAL_BOWL);
        if (bowlGos == null || bowlGos.Length == 0)
        {
            return new List<T>();
        }

        Dictionary<ulong, AnimalBowlSaveData> saveDataMap = new();
        if (bowlDataArr != null)
        {
            foreach (AnimalBowlSaveData saveData in bowlDataArr)
            {
                saveDataMap.Add(saveData.BowlId, saveData);
            }
        }

        List<T> bowlList = new();
        HashSet<ulong> bowlIdSet = new();
        foreach (GameObject bowlGo in bowlGos)
        {
            if (!bowlGo.TryGetComponent(out AnimalBowlConfig bowlConfig))
            {
                continue;
            }

            if (bowlIdSet.Contains(bowlConfig.BowlId))
            {
                Log.Error($"食盆id重复 bowlId:{bowlConfig.BowlId}");
                continue;
            }

            _ = bowlIdSet.Add(bowlConfig.BowlId);
            T bowl = bowlGo.AddComponent<T>();
            _ = saveDataMap.TryGetValue(bowlConfig.BowlId, out AnimalBowlSaveData saveData);
            saveData ??= new AnimalBowlSaveData() { BowlId = bowlConfig.BowlId };
            bowl.Data.SetSaveData(saveData);
            bowlList.Add(bowl);
        }

        return bowlList;
    }
}