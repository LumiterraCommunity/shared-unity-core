using UnityGameFramework.Runtime;


/// <summary>
/// 跟随后的宠物专属数据 挂在宠物实体上
/// </summary>
public class FollowingPetDataCore : PetDataCore
{
    /// <summary>
    /// 当前跟随的目标
    /// </summary>
    public EntityBase FollowingTarget { get; protected set; }
    /// <summary>
    /// 当前是否正在跟随
    /// </summary>
    public bool IsFollowing => FollowingTarget != null;
    /// <summary>
    /// 获取跟随技能ID列表,一定会返回一个数组
    /// </summary>
    /// <returns></returns>
    public int[] GetFollowingSkills()
    {
        if (!HasPetAbility(ePetAbility.SkillExtend) || !IsFollowing)
        {
            //没有扩展技能特性或者当前没在跟随
            return new int[] { };
        }

        if (PetCfg == null)
        {
            Log.Error("PetCfg is null");
            return new int[] { };
        }

        return PetCfg.ExtendSkill;
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    /// <param name="target"></param>
    public void SetFollowingTarget(EntityBase target)
    {
        if (FollowingTarget == target)
        {
            return;
        }

        FollowingTarget = target;
    }
}