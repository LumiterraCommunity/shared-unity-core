/*
 * @Author: xiang huan
 * @Date: 2022-12-02 10:52:02
 * @Description: 用于管理场景中的元素（元素不走实体管理那套，一般是指场景全局运行且需要同步数据的对象，谨慎使用，如果对象过多要同步的数据量巨大，应考虑使用实体走九宫格那套）
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/SceneElementMgrCore.cs
 * 
 */

//区域名字
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class SceneElementMgrCore : SceneModuleBase
{
    public Dictionary<long, SceneElementCore> SceneElementDic = new();
    public Dictionary<eSceneElementType, List<SceneElementCore>> SceneElementDicByType = new();
    protected void Awake()
    {
        SceneElementCore.OnSceneElementInitHook += AddSceneElement;
    }
    protected void OnDestroy()
    {
        SceneElementCore.OnSceneElementInitHook -= AddSceneElement;
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

    public List<T> GetSceneElementListByType<T>(eSceneElementType elementType) where T : SceneElementCore
    {
        if (SceneElementDicByType.TryGetValue(elementType, out List<SceneElementCore> elementList))
        {
            return elementList as List<T>;
        }
        return null;
    }
}