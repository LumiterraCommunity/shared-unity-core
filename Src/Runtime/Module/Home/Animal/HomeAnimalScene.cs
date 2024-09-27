using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 畜牧场景管理
/// </summary>
public class HomeAnimalScene : MonoBehaviour
{
    /// <summary>
    /// 动物活动区域x z的范围
    /// </summary>
    /// <value></value>
    public Rect PlaygroundRect { get; private set; }
    /// <summary>
    /// 动物活动区域尺寸
    /// </summary>
    /// <value></value>
    public Vector3 PlaygroundSize { get; private set; }
    /// <summary>
    /// 动物活动区域中心点
    /// </summary>
    /// <value></value>
    public Vector3 PlaygroundCenter { get; private set; }

    /// <summary>
    /// 所有食盆 顺序为吃的顺序
    /// </summary>
    private readonly ListMap<ulong, AnimalBowlCore> _bowlListMap = new();

    private void Start()
    {
        GameObject homeSceneConfigGo = GameObject.FindWithTag(MTag.HOME_ANIMAL_SCENE_CONFIG);
        AnimalSceneConfig sceneConfig = homeSceneConfigGo == null ? null : homeSceneConfigGo.GetComponent<AnimalSceneConfig>();
        if (sceneConfig == null)
        {
            if (HomeModuleCore.IsInited && HomeModuleCore.HomeData.IsPersonalHome)//副本家园暂时没有动物配置 将来也许有 先不报错
            {
                Log.Error("场景中没有AnimalSceneConfig");
            }
            return;
        }

        PlaygroundSize = sceneConfig.PlaygroundSize;
        PlaygroundCenter = sceneConfig.transform.position;
        float x = PlaygroundCenter.x - (PlaygroundSize.x * 0.5f);
        float z = PlaygroundCenter.z - (PlaygroundSize.z * 0.5f);
        PlaygroundRect = new Rect(x, z, PlaygroundSize.x, PlaygroundSize.z);
    }

    private void OnDestroy()
    {
        foreach (AnimalBowlCore bowlCore in _bowlListMap)
        {
            Destroy(bowlCore);//只删除自己脚本 是自己添加的脚步
        }
        _bowlListMap.Clear();
    }

    /// <summary>
    /// 添加食盆 需要排好序 顺序为吃的顺序
    /// </summary>
    /// <param name="bowlCore"></param>
    internal void AddBowls(AnimalBowlCore bowlCore)
    {
        _ = _bowlListMap.Add(bowlCore.Data.SaveData.BowlId, bowlCore);
    }

    /// <summary>
    /// 找到最近的一个有食物的食盆 找不到返回null
    /// </summary>
    /// <returns></returns>
    public AnimalBowlCore SearchNearestHaveFoodBowl(Vector3 pos)
    {
        AnimalBowlCore result = null;
        float minDis = float.MaxValue;

        for (int i = 0; i < _bowlListMap.Count; i++)
        {
            if (_bowlListMap[i].Data.IsHaveFood)
            {
                float dis = Vector3.Distance(pos, _bowlListMap[i].Position);
                if (dis < minDis)
                {
                    minDis = dis;
                    result = _bowlListMap[i];
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 通过ID获取食盆
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AnimalBowlCore GetBowl(ulong id)
    {
        return _bowlListMap.GetFromKey(id);
    }

    /// <summary>
    /// 按照顺序 获取所有食盆
    /// </summary>
    /// <returns></returns>
    public AnimalBowlCore[] GetBowls()
    {
        return _bowlListMap.ToArray();
    }
}