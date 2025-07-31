using System.Collections.Generic;
using System.Linq;
using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 实体技能采集器，负责采集各个模块提供的技能并添加到实体上
/// </summary>
public class PlayerSkillCollector : EntitySkillCollector
{
    private bool _isInitTalentSkill = false;
    private PlayerRoleDataCore _playerRoleDataCore;
    protected PlayerRoleDataCore RoleDataCore
    {
        get
        {
            if (_playerRoleDataCore == null)
            {
                _playerRoleDataCore = gameObject.GetComponent<PlayerRoleDataCore>();
            }
            return _playerRoleDataCore;
        }
    }

    protected override void Start()
    {
        base.Start();
        //天赋树
        RefEntity.EntityEvent.TalentSkillUpdated += OnTalentSkillUpdated;
        RefEntity.EntityEvent.TalentSkillInited += OnTalentSkillInited;
        RefEntity.EntityEvent.EntityDataInitFinish += CheckSkillCollector;

        //宠物跟随技能
        if (RefEntity.TryGetComponent(out PlayerPetMgrCore playerPetData))
        {
            playerPetData.PetFollow += OnPetFollow;
            playerPetData.PetUnFollow += OnPetUnFollow;

        }
        CheckSkillCollector();
    }
    public void CheckSkillCollector()
    {
        //宠物跟随技能
        if (RefEntity.TryGetComponent(out PlayerPetMgrCore playerPetData))
        {
            CheckPetSkill();
        }

        CheckInitTalentSkill();
        //装备
        OnUpdateEquipmentSkillID();
        //手持物品
        OnInHandItemChanged(RefEntity.GetComponent<EntityInHandItemDataCore>().Cid);

        CollectFromItem();

        InitCustomSkill();
    }
    protected override void OnDestroy()
    {
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.TalentSkillUpdated -= OnTalentSkillUpdated;
            RefEntity.EntityEvent.TalentSkillInited -= OnTalentSkillInited;
            RefEntity.EntityEvent.EntityDataInitFinish -= CheckSkillCollector;

            if (RefEntity.TryGetComponent(out PlayerPetMgrCore playerPetData))
            {
                playerPetData.PetFollow -= OnPetFollow;
                playerPetData.PetUnFollow -= OnPetUnFollow;
            }
        }

        base.OnDestroy();
    }

    private void OnTalentSkillUpdated(IEnumerable<int> addList, IEnumerable<int> removeList)
    {
        if (addList != null)
        {
            foreach (int skillID in addList)
            {
                AddTalentSkill(skillID);
            }
        }

        if (removeList != null)
        {

            foreach (int skillID in removeList)
            {
                RemoveTalentSkill(skillID);
            }
        }
        EntitySkillDataCore.BroadCastUpdateSkillGroup(eSkillGroupType.Talent);
    }

    private void OnTalentSkillInited(IEnumerable<int> skills)
    {
        InitTalentSkill(skills);
    }

    public void AddTalentSkill(int skillID)
    {
        if (EntitySkillDataCore == null)
        {
            return;
        }
        DRSkill skillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (skillCfg != null)
        {
            foreach (int skill in skillCfg.ComposeSkill)
            {
                EntitySkillDataCore.AddSkillGroupID(eSkillGroupType.Talent, skill);//添加组合技能
            }
        }
        EntitySkillDataCore.AddSkillGroupID(eSkillGroupType.Talent, skillID);
    }

    public void RemoveTalentSkill(int skillID)
    {
        if (EntitySkillDataCore == null)
        {
            return;
        }

        DRSkill skillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (skillCfg != null)
        {
            foreach (int skill in skillCfg.ComposeSkill)
            {
                EntitySkillDataCore.RemoveSkillGroupID(eSkillGroupType.Talent, skill);//组合技能
            }
        }
        EntitySkillDataCore.RemoveSkillGroupID(eSkillGroupType.Talent, skillID);
    }

    private void CheckInitTalentSkill()
    {
        PlayerTalentTreeDataCore talentData = RefEntity.GetComponent<PlayerTalentTreeDataCore>();
        if (talentData != null)
        {
            InitTalentSkill(talentData.GetTalentSkills());
        }
    }

    private void InitTalentSkill(IEnumerable<int> skills)
    {
        if (_isInitTalentSkill || skills.Count() == 0)
        {
            return;
        }
        _isInitTalentSkill = true;

        foreach (int skillID in skills)
        {
            AddTalentSkill(skillID);
        }

        EntitySkillDataCore.BroadCastUpdateSkillGroup(eSkillGroupType.Talent);
    }

    protected override void OnUpdateEquipmentSkillID()
    {
        EntityAvatarDataCore avatarData = RefEntity.GetComponent<EntityAvatarDataCore>();
        if (avatarData == null)
        {
            return;
        }

        if (avatarData.AvatarDic.TryGetValue(AvatarPosition.Weapon, out AvatarAttribute avatar))
        {
            DREquipment drEquipment = EquipmentTable.Inst.GetRowByItemID(avatar.ObjectId);
            if (drEquipment != null)
            {
                EntitySkillDataCore.SetSkillGroupIDList(eSkillGroupType.Equipment, drEquipment.GivenSkillId);
            }
            else
            {
                Log.Error($"OnUpdateEquipmentSkillID not find equipment id:{avatar.ObjectId}");
            }
        }
        else
        {
            EntitySkillDataCore.RemoveSkillGroupIDList(eSkillGroupType.Equipment);
        }
    }

    /// <summary>
    /// 外部debug测试强制设置装备技能
    /// </summary>
    /// <param name="equipmentItemID"></param>
    public void DebugTestForceSetEquipmentSkill(int equipmentItemID)
    {
        DREquipment drEquipment = EquipmentTable.Inst.GetRowByItemID(equipmentItemID);
        if (drEquipment != null)
        {
            EntitySkillDataCore.SetSkillGroupIDList(eSkillGroupType.Equipment, drEquipment.GivenSkillId);
        }
    }

    /// <summary>
    /// 当前跟随的宠物发生变化
    /// </summary>
    /// <param name="pet"></param>
    private void OnPetFollow(EntityBase _)
    {
        CheckPetSkill();
    }

    private void OnPetUnFollow(EntityBase _)
    {
        CheckPetSkill();
    }

    private void CheckPetSkill()
    {
        EntitySkillDataCore.RemoveSkillGroupIDList(eSkillGroupType.Pet);
        PlayerPetMgrCore petMgr = RefEntity.GetComponent<PlayerPetMgrCore>();
        if (petMgr == null)
        {
            Log.Error($"CollectSkillFromPet error, petMgr is null");
            return;
        }

        if (petMgr.FollowingPet == null)
        {
            //当前没有宠物跟随
            return;
        }

        PetFollowDataCore followData = petMgr.FollowingPet.GetComponent<PetFollowDataCore>();
        if (followData == null)
        {
            Log.Error($"CollectSkillFromPet error, petData is null");
            return;
        }

        int[] followSkills = followData.GetFollowingSkills();
        EntitySkillDataCore.SetSkillGroupIDList(eSkillGroupType.Pet, followSkills);
    }

    /// <summary>
    /// 从可释放技能的道具采集
    /// 这里不会检测玩家是否有这个道具，真正释放的时候会校验
    /// </summary>
    private void CollectFromItem()
    {
        List<int> skillList = new();
        DRItemEatable[] allItemEatable = GFEntryCore.DataTable.GetDataTable<DRItemEatable>().GetAllDataRows();//目前可使用道具才有配置技能
        string releaseSkillType = eFoodItemInteractType.releaseSkill.ToString();
        for (int i = 0; i < allItemEatable.Length; i++)
        {
            DRItemEatable item = allItemEatable[i];
            if (item.InteractType == releaseSkillType)
            {
                for (int j = 0; j < item.Args.Length; j++)
                {
                    if (int.TryParse(item.Args[j], out int skillID))
                    {
                        skillList.Add(skillID);
                    }
                }
            }
        }

        EntitySkillDataCore.SetSkillGroupIDList(eSkillGroupType.Item, skillList.ToArray());
    }

    private void InitCustomSkill()
    {
        EntitySkillDataCore.AddSkillGroupID(eSkillGroupType.General, (int)eSkillId.TotemTp);
    }


}