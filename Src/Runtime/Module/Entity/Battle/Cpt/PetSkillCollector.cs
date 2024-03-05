using System;


/// <summary>
/// 宠物技能采集
/// </summary>
public class PetSkillCollector : EntitySkillCollector
{
    protected override void Start()
    {
        base.Start();
        OnUpdateEquipmentSkillID();
        OnInHandItemChanged(RefEntity.GetComponent<EntityInHandItemDataCore>().Cid);


        CollectFromPetAbility();//采集宠物特性对应的技能
    }

    protected override void OnUpdateEquipmentSkillID()
    {
        //TODO:更新宠物的装备技能
    }

    /// <summary>
    /// 收集宠物特性对应的技能
    /// </summary>
    private void CollectFromPetAbility()
    {
        PetDataCore petData = RefEntity.GetComponent<PetDataCore>();
        if (petData == null)
        {
            return;
        }

        //遍历 ePetAbility
        foreach (ePetAbility ability in Enum.GetValues(typeof(ePetAbility)))
        {
            if (petData.AbilityToSkillIdDic.TryGetValue(ability, out int skillId))
            {
                EntitySkillDataCore.AddSkillGroupID(eSkillGroupType.Base, skillId);//特性是伴随宠物的，所以都是基础技能
            }
        }
    }
}