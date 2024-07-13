using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 实体手持物数据
/// </summary>
public class EntityInHandItemDataCore : EntityBaseComponent
{
    public bool HasInHandItem => !string.IsNullOrEmpty(Id);
    public string Id { get; private set; }//nft id,也是唯一id
    public int Cid { get; private set; }//道具配置id
    public int Count { get; private set; }//数量

    public void InitFromNetData(NftBaseInfo data)
    {
        if (data == null)
        {
            Log.Error("InitFromNetData data is null");
            return;
        }

        Init(data.NftId, data.Cid, data.Num);
    }

    // public void InitFromNetData(NftBaseInfo data)
    // {
    //     if (data == null)
    //     {
    //         Log.Error("InitFromNetData data is null");
    //         return;
    //     }

    //     Init(data.NftId, data.Cid, data.Num);
    // }

    private void Init(string id, int cid, int count)
    {
        if (Id == id)
        {
            return;
        }

        Id = id;
        Cid = string.IsNullOrEmpty(id) ? 0 : cid;
        Count = count;

        RefEntity.EntityEvent.InHandItemChange?.Invoke(Cid);
    }

    public NftBaseInfo ToNetData()
    {
        return new()
        {
            NftId = Id,
            Cid = Cid,
            Num = Count
        };
    }
}