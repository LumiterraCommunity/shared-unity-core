using System;
using System.Collections.Generic;
using System.Linq;
using GameMessageCore;
using UnityGameFramework.Runtime;

public class EntityAvatarDataCore : EntityBaseComponent
{
    /// <summary>
    /// 所有装备等级平均值 注意是根据所有槽数量计算的并不是当前已装备的 会给小数 用于精确计算 外面需要取整自己决定
    /// </summary>
    /// <value></value>
    public float AllEquipmentsLevelAvg { get; protected set; } = 0;

    /// <summary>
    /// 角色穿着数据
    /// 用字典方便业务层使用
    /// </summary>
    /// <value></value>
    public Dictionary<AvatarPosition, AvatarAttribute> AvatarDic { get; protected set; } = new();
    /// <summary>
    /// 角色穿着列表
    /// </summary>
    public List<AvatarAttribute> AvatarList { get; protected set; } = new();

    /// <summary>
    /// 从网络数据初始化
    /// </summary>
    /// <param name="avatars"></param>
    public void InitFromNet(GrpcAvatarAttribute[] avatars)
    {
        AvatarDic.Clear();
        AvatarList.Clear();
        if (avatars != null && avatars.Length > 0)
        {
            foreach (GrpcAvatarAttribute avatar in avatars)
            {
                AvatarAttribute avatarAttribute = avatar.ToProtoData();
                AvatarDic.Add(avatarAttribute.Position, avatarAttribute);
            }
            AvatarList.AddRange(AvatarDic.Values);
        }
        OnAvatarUpdate();
    }

    public void InitFromNet(IEnumerable<AvatarAttribute> avatars)
    {
        AvatarDic.Clear();
        AvatarList.Clear();
        AvatarList.AddRange(avatars);
        foreach (AvatarAttribute avatar in avatars)
        {
            AvatarDic.Add(avatar.Position, avatar);
        }
        OnAvatarUpdate();
    }

    /// <summary>
    /// 更新了avatar 单个和全部都需要调用这里
    /// </summary>
    private void OnAvatarUpdate()
    {
        CalculateAllEquipmentsLevelAvg();

        RefEntity.EntityEvent.EntityAvatarUpdated?.Invoke();
    }

    private void CalculateAllEquipmentsLevelAvg()
    {
        if (AvatarList == null || AvatarList.Count == 0)
        {
            AllEquipmentsLevelAvg = 0;
            return;
        }

        float allLv = 0;
        foreach (AvatarAttribute avatar in AvatarList)
        {
            //不是装备的不计算 比如外观
            if (!AvatarDefineCore.EquipmentPartList.Contains(avatar.Position))
            {
                continue;
            }

            DRItem drItem = TableUtil.GetConfig<DRItem>(avatar.ObjectId);
            if (drItem == null)
            {
                Log.Error($"CalculateAllEquipmentsLevelAvg drItem is null,avatar cid:{avatar.ObjectId}");
                continue;
            }

            allLv += drItem.UseLv;
        }

        AllEquipmentsLevelAvg = allLv / AvatarDefineCore.EquipmentPartList.Count;
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