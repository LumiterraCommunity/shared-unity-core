/*
 * @Author: xiang huan
 * @Date: 2022-12-02 10:52:02
 * @Description: 用于管理场景中的元素（元素不走实体管理那套，一般是指场景全局运行且需要同步数据的对象，谨慎使用，如果对象过多要同步的数据量巨大，应考虑使用实体走九宫格那套）
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/SceneElementMgrCore.cs
 * 
 */

//区域名字
using System.Collections.Generic;
using GameMessageCore;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;

public class SceneElementMgrCore : SceneModuleBase
{
    public Dictionary<long, SceneElementCore> SceneElementDic = new();
    public Dictionary<eSceneElementType, List<SceneElementCore>> SceneElementDicByType = new();
    protected List<SceneElementData> SceneElementDataList = new();
    protected virtual void Awake()
    {
        SceneElementCore.OnSceneElementInitHook += AddSceneElement;
        SceneElementCore.OnSceneElementDestroyHook += RemoveSceneElement;
    }
    protected virtual void OnDestroy()
    {
        SceneElementCore.OnSceneElementInitHook -= AddSceneElement;
        SceneElementCore.OnSceneElementDestroyHook -= RemoveSceneElement;
    }
    public void AddSceneElement(long id, SceneElementCore sceneElement)
    {
        if (SceneElementDic.ContainsKey(id))
        {
            Log.Error($"场景元素ID重复 {id}");
            return;
        }
        SceneElementDic.Add(id, sceneElement);
        if (SceneElementDicByType.TryGetValue(sceneElement.ElementType, out List<SceneElementCore> elementList))
        {
            elementList.Add(sceneElement);
        }
        else
        {
            elementList = new()
            {
                sceneElement
            };
            SceneElementDicByType.Add(sceneElement.ElementType, elementList);
        }
    }
    public void RemoveSceneElement(long id)
    {
        if (SceneElementDic.TryGetValue(id, out SceneElementCore sceneElement))
        {
            _ = SceneElementDic.Remove(id);
            if (SceneElementDicByType.TryGetValue(sceneElement.ElementType, out List<SceneElementCore> elementList))
            {
                _ = elementList.Remove(sceneElement);
            }
        }
    }

    public T GetSceneElement<T>(long id) where T : SceneElementCore
    {
        if (SceneElementDic.TryGetValue(id, out SceneElementCore sceneElement))
        {
            return (T)sceneElement;
        }
        return null;
    }

    /// <summary>
    /// 有性能开销，不要频繁调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elementType"></param>
    /// <returns></returns> <summary>
    public List<T> GetSceneElementListByType<T>(eSceneElementType elementType) where T : SceneElementCore
    {
        if (SceneElementDicByType.TryGetValue(elementType, out List<SceneElementCore> elementList))
        {
            return elementList.ConvertAll(x => (T)x);
        }
        return null;
    }

    /// <summary>
    /// 根据类型和区域ID获取场景元素列表
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="areaID"></param>
    /// <param name="resultList"></param> <summary>
    public void GetSceneElementListByTypeAndAreaID(eSceneElementType elementType, int areaID, List<SceneElementCore> resultList)
    {
        if (SceneElementDicByType.TryGetValue(elementType, out List<SceneElementCore> elementList))
        {
            for (int i = 0; i < elementList.Count; i++)
            {
                if (elementList[i].AreaID == areaID)
                {
                    resultList.Add(elementList[i]);
                }
            }
        }
    }

    public void InitNetData(RepeatedField<SceneElementData> sceneElementDataList)
    {
        for (int i = 0; i < sceneElementDataList.Count; i++)
        {
            SceneElementData element = sceneElementDataList[i];
            if (SceneElementDic.TryGetValue(element.Id, out SceneElementCore sceneElement))
            {
                sceneElement.InitElementData(element);
            }
        }
    }
    public List<SceneElementData> GetNetData()
    {
        SceneElementDataList.Clear();
        if (SceneElementDic.Count == 0)
        {
            return SceneElementDataList;
        }
        foreach (KeyValuePair<long, SceneElementCore> item in SceneElementDic)
        {
            if (item.Value.IsSyncData)
            {
                SceneElementDataList.Add(item.Value.GetElementData());
            }
        }
        return SceneElementDataList;
    }
}