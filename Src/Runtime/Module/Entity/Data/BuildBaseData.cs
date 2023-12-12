
using GameMessageCore;
/// <summary>
/// 建造物数据信息
/// </summary>
public class BuildBaseData : EntityBaseComponent
{
    public NftBuild NftBuild;

    public virtual void SetData(NftBuild nftBuild)
    {
        NftBuild = nftBuild;
    }
}