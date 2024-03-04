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
    public IEnumerable<AvatarAttribute> AvatarList { get; protected set; }


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
}