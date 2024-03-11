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

    private void Start()
    {
        InitPetDataCore();
    }

    private void InitPetDataCore()
    {
        if (_petDataCore == null)
        {
            _petDataCore = RefEntity.GetComponent<PetDataCore>();
        }
    }

    /// <summary>
    /// 获取跟随技能ID列表,一定会返回一个数组
    /// </summary>
    /// <returns></returns>
    public int[] GetFollowingSkills()
    {
        if (_petDataCore == null)
        {
            InitPetDataCore();
        }

        if (_petDataCore == null)
        {
            Log.Error("PetDataCore is null");
            return new int[] { };
        }

        if (!_petDataCore.HasPetAbility(ePetAbility.SkillExtend) || !_petDataCore.IsFollowing)
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
}