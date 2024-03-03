using UnityGameFramework.Runtime;


/// <summary>
/// 宠物的跟随数据
/// </summary>
public class PetFollowDataCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物核心数据
    /// </summary>
    private PetDataCore _petDataCore;
    /// <summary>
    /// 当前跟随的目标
    /// </summary>
    public EntityBase FollowingTarget
    { get; protected set; }
    /// <summary>
    /// 当前是否正在跟随
    /// </summary>
    public bool IsFollowing => FollowingTarget != null;
    private void Start()
    {
        _petDataCore = RefEntity.GetComponent<PetDataCore>();
    }
    /// <summary>
    /// 获取跟随技能ID列表,一定会返回一个数组
    /// </summary>
    /// <returns></returns>
    public int[] GetFollowingSkills()
    {
        if (_petDataCore == null)
        {
            //可能还没Start，这里要判断一下
            return new int[] { };
        }

        if (!_petDataCore.HasPetAbility(ePetAbility.SkillExtend) || !IsFollowing)
        {
            //没有扩展技能特性或者当前没在跟随
            return new int[] { };
        }

        if (_petDataCore.PetCfg == null)
        {
            Log.Error("PetCfg is null");
            return new int[] { };
        }

        return _petDataCore.PetCfg.ExtendSkill;
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