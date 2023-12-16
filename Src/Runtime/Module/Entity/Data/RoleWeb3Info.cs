/// <summary>
/// 角色web3相关信息
/// </summary>
public class RoleWeb3Info : EntityBaseComponent
{
    /// <summary>
    /// 盲盒质押登记，0~4级，0为未质押
    /// </summary>
    public int BoxStakeLv { get; private set; } = 0;
    /// <summary>
    /// kol邀请码
    /// </summary>
    public string KolInviteCode { get; private set; } = "";
    /// <summary>
    /// kol组织,取自kol邀请码中间部分
    /// </summary>
    public string KolOrganization { get; private set; } = "";

    public void SetBoxStakeLv(int lv)
    {
        BoxStakeLv = lv;
    }

    public void SetKolInviteCode(string code)
    {
        KolInviteCode = code;
        string[] codeSplits = code.Split('-');
        if (codeSplits.Length == 3)
        {
            KolOrganization = codeSplits[1];
        }
    }

    public string GetBoxStakeLvColor()
    {
        switch (BoxStakeLv)
        {
            case 1:
                return "#eab766";
            case 2:
                return "#64abcf";
            case 3:
                return "#ac84a9";
            case 4:
                return "#f9f5bc";
            case 0:
            default:
                return "#FFFFFF";
        }
    }
}