/// <summary>
/// 宠物技能采集
/// </summary>
public class PetSkillCollector : EntitySkillCollector
{
    protected override void Start()
    {
        base.Start();
        OnUpdateEquipmentSkillID();
        OnInHandItemChanged(RefEntity.GetComponent<PetDataCore>().InHandItem);
    }

    protected override void OnUpdateEquipmentSkillID()
    {
        //TODO:更新宠物的装备技能
    }
}