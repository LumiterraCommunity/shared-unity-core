using GameMessageCore;
/// <summary>
/// 动物基础数据 用于数据管理面板的数据
/// </summary>
[System.Serializable]
public class AnimalBaseData
{
    /// <summary>
    /// 动物全局Id 和存档数据id一致
    /// </summary>
    public ulong AnimalId;
    /// <summary>
    /// 玩家取得名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 动物配置id
    /// </summary>
    public int Cid;
    /// <summary>
    /// 好感度数值
    /// </summary>
    public int Favorability;
    /// <summary>
    /// 动物创建时间戳
    /// </summary>
    public long CreateMs;
    /// <summary>
    /// 动物最近更新时间戳
    /// </summary>
    public long UpdateMs;

    public AnimalBaseData()
    {

    }

    public AnimalBaseData(ProxyAnimalBaseData data)
    {
        AnimalId = data.AnimalId;
        Name = data.Name;
        Cid = data.Cid;
        Favorability = data.FavorAbility;
        CreateMs = data.CreateMs;
        UpdateMs = data.UpdateMs;
    }

    public ProxyAnimalBaseData ToProxyAnimalBaseData()
    {
        return new ProxyAnimalBaseData()
        {
            AnimalId = AnimalId,
            Name = Name,
            Cid = Cid,
            FavorAbility = Favorability,
            CreateMs = CreateMs,
            UpdateMs = UpdateMs,
        };
    }
}