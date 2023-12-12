using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 实体技能采集器，负责采集各个模块提供的技能并添加到实体上
/// </summary>
public class EntitySkillCollector : EntityBaseComponent
{
    protected EntitySkillDataCore EntitySkillDataCore;
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

    private void Start()
    {
        EntitySkillDataCore = RefEntity.GetComponent<EntitySkillDataCore>();
        //天赋树
        RefEntity.EntityEvent.TalentSkillUpdated += OnTalentSkillUpdated;
        RefEntity.EntityEvent.TalentSkillInited += OnTalentSkillInited;
        CheckInitTalentSkill();

        //装备
        RefEntity.EntityEvent.EntityAvatarUpdated += OnUpdateEquipmentSkillID;
        OnUpdateEquipmentSkillID();
    }

    private void OnDestroy()
    {
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.TalentSkillUpdated -= OnTalentSkillUpdated;
            RefEntity.EntityEvent.TalentSkillInited -= OnTalentSkillInited;
            RefEntity.EntityEvent.EntityAvatarUpdated -= OnUpdateEquipmentSkillID;
        }
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

    protected void OnUpdateEquipmentSkillID()
    {
        if (RoleDataCore == null)
        {
            return;
        }
        if (RoleDataCore.WearDic.TryGetValue(AvatarPosition.Weapon, out AvatarAttribute avatar))
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
}