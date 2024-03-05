using System.Collections.Generic;
using GameMessageCore;

public class EntityAvatarDataCore : EntityBaseComponent
{
    /// <summary>
    /// 角色穿着数据
    /// 用字典方便业务层使用
    /// </summary>
    /// <value></value>
    public Dictionary<AvatarPosition, AvatarAttribute> AvatarDic { get; protected set; } = new();
    /// <summary>
    /// 角色穿着列表
    /// </summary>
    public IEnumerable<AvatarAttribute> AvatarList { get; protected set; }

    /// <summary>
    /// 设置角色穿着数据
    /// </summary>
    /// <param name="avatars"></param>
    public void SetRoleAvatars(IEnumerable<AvatarAttribute> avatars)
    {
        AvatarDic.Clear();
        AvatarList = avatars;
        foreach (AvatarAttribute avatar in avatars)
        {
            AvatarDic.Add(avatar.Position, avatar);
        }

        RefEntity.EntityEvent.EntityAvatarUpdated?.Invoke();
    }

    /// <summary>
    /// 获取武器的ItemCid 没有装备返回-1
    /// </summary>
    /// <returns></returns>
    public int GetWeaponAvatar()
    {
        if (!AvatarDic.TryGetValue(AvatarPosition.Weapon, out AvatarAttribute weaponAvatar))
        {
            return -1;
        }

        return weaponAvatar.ObjectId;
    }
}