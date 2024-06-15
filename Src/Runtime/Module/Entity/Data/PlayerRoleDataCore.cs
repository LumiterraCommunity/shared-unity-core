/** 
 * @Author XQ
 * @Date 2022-08-09 09:51:55
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/PlayerRoleDataCore.cs
 */
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 玩家场景角色数据
/// </summary>
public class PlayerRoleDataCore : EntityBaseComponent
{
    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    public string Gender { get; protected set; }

    /// <summary>
    /// 角色配置ID
    /// </summary>
    /// <value></value>
    public int RoleCfgID => DRRole == null ? -1 : DRRole.Id;

    /// <summary>
    /// 角色配置表
    /// </summary>
    /// <value></value>
    public DRRole DRRole { get; protected set; }

    /// <summary>
    /// 外观数据
    /// </summary>
    /// <value></value>
    public PlayerFeature RoleFeature { get; protected set; }

    /// <summary>
    /// 精力值
    /// </summary>
    /// <returns></returns>
    public float Energy => RefEntity.EntityAttributeData.GetRealValue(eAttributeType.Energy);

    /// <summary>
    /// 最大精力值
    /// </summary>
    /// <returns></returns>
    public float MaxEnergy => RefEntity.EntityAttributeData.GetRealValue(eAttributeType.MaxEnergy);

    /// <summary>
    /// 自动恢复精力是否被锁住
    /// </summary>
    /// <value></value>
    public bool IsLockAutoRecovery { get; private set; }

    /// <summary>
    /// 角色邀请码
    /// 先放在这里，节省一个组件的开销
    /// </summary>
    public string InviteCode { get; protected set; } = string.Empty;
    public string InviteCodeOrganization { get; protected set; } = string.Empty;
    /// <summary>
    /// 角色盲盒质押等级，0~4级，0为未质押
    /// </summary>
    public int BoxStakeLv { get; protected set; } = 0;
    /// <summary>
    /// 地图ID
    /// </summary>
    public int MapId { get; protected set; }

    public void SetGender(string gender)
    {
        Gender = gender;
    }

    public void SetRoleCfgID(int roleCfgID)
    {
        DRRole = GFEntryCore.DataTable.GetDataTable<DRRole>().GetDataRow(roleCfgID);

        if (DRRole == null)
        {
            Log.Error($"Can not find role cfg id:{roleCfgID}");
        }
    }

    public void SetRoleFeature(PlayerFeature feature)
    {
        RoleFeature = feature;
    }

    /// <summary>
    /// 设置精力值
    /// </summary>
    /// <param name="energy"></param>
    /// <param name="isRealValue">是real经过单位换算的还是原始单位的 非real会强转int</param>
    public void SetEnergy(float energy, bool isRealValue)
    {
        if (isRealValue)
        {
            RefEntity.EntityAttributeData.SetRealBaseValue(eAttributeType.Energy, energy);
        }
        else
        {
            RefEntity.EntityAttributeData.SetBaseValue(eAttributeType.Energy, (int)energy);
        }
    }

    /// <summary>
    ///  设置最大精力值
    /// </summary>
    /// <param name="maxEnergy"></param>
    /// <param name="isRealValue">是real经过单位换算的还是原始单位的 非real会强转int</param>
    public void SetMaxEnergy(float maxEnergy, bool isRealValue)
    {
        if (isRealValue)
        {
            RefEntity.EntityAttributeData.SetRealBaseValue(eAttributeType.MaxEnergy, maxEnergy);
        }
        else
        {
            RefEntity.EntityAttributeData.SetBaseValue(eAttributeType.MaxEnergy, (int)maxEnergy);
        }
    }

    public void SetLockAutoRecovery(bool isLock)
    {
        IsLockAutoRecovery = isLock;
        Log.Info($"player {RefEntity.BaseData.Id} SetLockAutoRecovery:{isLock}");
    }

    public void SetInviteCode(string inviteCode)
    {
        if (string.IsNullOrEmpty(inviteCode))
        {
            return;
        }

        string[] codeSplits = inviteCode.Split('-');
        if (codeSplits.Length == 3)
        {
            InviteCodeOrganization = codeSplits[1];
        }

        InviteCode = inviteCode;
    }

    public void SetBoxStakeLv(int lv)
    {
        BoxStakeLv = lv;
    }

    public void SetMapId(int mapId)
    {
        MapId = mapId;
    }
}