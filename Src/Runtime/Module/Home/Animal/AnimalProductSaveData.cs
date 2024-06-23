using GameMessageCore;
/// <summary>
/// 自动生产的动物生产下来的产品存档数据
/// </summary>
[System.Serializable]
public class AnimalProductSaveData
{
    /// <summary>
    /// 产品全局Id
    /// </summary>
    public long ProductId;
    /// <summary>
    /// 道具配置id
    /// </summary>
    public int ItemCid;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int ItemNum;
    /// <summary>
    /// 品质
    /// </summary>
    public int Quality;

    /// <summary>
    /// 掉落位置
    /// </summary>
    public Vector3 Pos;

    public AnimalProductSaveData()
    {

    }

    public AnimalProductSaveData(ProxyProductData data)
    {
        ProductId = data.ProductId;
        ItemCid = data.ItemCid;
        ItemNum = data.ItemNum;
        Quality = data.Quality;
        Pos = data.Position;
    }

    public ProxyProductData ToProxyAnimalData()
    {
        return new()
        {
            ProductId = ProductId,
            ItemCid = ItemCid,
            ItemNum = ItemNum,
            Quality = Quality,
            Position = Pos,
        };
    }
}