using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

public class EntityAvatarDataCore : EntityBaseComponent
{
    /// <summary>
    /// 当前能力等级的所属天赋类型 由所持武器决定
    /// </summary>
    /// <value></value>
    private eTalentType _curAbilityType = eTalentType.battle;
    private readonly Dictionary<eTalentType, float> _abilityLevelMap = new();

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
    /// 获取专精能力等级 注意是根据所有槽数量计算的并不是当前已装备的 会给小数 用于精确计算 外面需要取整自己决定
    /// </summary>
    /// <param name="talentType"></param>
    /// <returns></returns>
    public float GetAbilityLevel(eTalentType talentType)
    {
        return _abilityLevelMap.GetValueOrDefault(talentType, 0);
    }

    /// <summary>
    /// 获取当前专精能力等级 谨慎使用 注意是根据所有槽数量计算的并不是当前已装备的 会给小数 用于精确计算 外面需要取整自己决定
    /// </summary>
    /// <returns></returns>
    public float GetCurAbilityLevel()
    {
        return GetAbilityLevel(_curAbilityType);
    }

    /// <summary>
    /// 从网络数据初始化
    /// </summary>
    /// <param name="avatars"></param>
    public void InitFromNet(AvatarAttribute[] avatars)
    {
        AvatarDic.Clear();
        AvatarList.Clear();
        if (avatars != null && avatars.Length > 0)
        {
            foreach (AvatarAttribute avatar in avatars)
            {
                AvatarAttribute avatarAttribute = avatar;
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
        UpdateCurAbilityType();

        CalculateAbilityLevel();

        RefEntity.EntityEvent.EntityAvatarUpdated?.Invoke();
    }

    /// <summary>
    /// 更新当前专精类型 由武器决定
    /// </summary>
    private void UpdateCurAbilityType()
    {
        _curAbilityType = eTalentType.battle;

        if (AvatarList == null || AvatarList.Count == 0)
        {
            return;
        }

        if (AvatarDic.TryGetValue(AvatarPosition.Weapon, out AvatarAttribute weaponAvatar))
        {
            DRItem drWeapon = TableUtil.GetConfig<DRItem>(weaponAvatar.ObjectId);
            if (drWeapon != null && drWeapon.TalentId.Length > 0)
            {
                _curAbilityType = (eTalentType)drWeapon.TalentId[0];
            }
            else
            {
                Log.Error($"CalculateAbilityLevel drWeapon is null or not have talent,avatar cid:{weaponAvatar.ObjectId}");
            }
        }
    }

    /// <summary>
    /// 重新计算出所有天赋的能力等级
    /// </summary>
    private void CalculateAbilityLevel()
    {
        _abilityLevelMap.Clear();

        if (AvatarList == null || AvatarList.Count == 0)
        {
            return;
        }

        //算总和
        Dictionary<eTalentType, float> lvSumMap = new();//value 某个天赋的等级总和
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
                Log.Error($"CalculateAbilityLevel drItem is null,avatar cid:{avatar.ObjectId}");
                continue;
            }

            //没有天赋
            if (drItem.TalentId == null || drItem.TalentId.Length == 0)
            {
                continue;
            }

            //多个天赋
            foreach (int talentId in drItem.TalentId)
            {
                eTalentType talentType = (eTalentType)talentId;
                float curLv = lvSumMap.GetValueOrDefault(talentType, 0);
                lvSumMap[talentType] = curLv + drItem.UseLv;
            }
        }

        //算平均到结果
        foreach (KeyValuePair<eTalentType, float> item in lvSumMap)
        {
            _abilityLevelMap[item.Key] = (float)item.Value / AvatarDefineCore.EquipmentPartList.Count;//除所有装备槽数量
        }
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