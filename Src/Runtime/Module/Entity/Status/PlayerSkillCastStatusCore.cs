using System;
using GameFramework.Fsm;

/// <summary>
/// 玩家技能cast状态core库
/// </summary>
public class PlayerSkillCastStatusCore : SkillCastStatusCore
{
    protected EntitySkillDataCore SkillDataCore { get; private set; }
    private bool _curSkillIsComboGroupSkill;//当前技能是否连击组 往往代表普通攻击

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        SkillDataCore = StatusCtrl.GetComponent<EntitySkillDataCore>();
        _curSkillIsComboGroupSkill = SkillDataCore.IsComboGroupSkill(SkillID);
    }
    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        SkillDataCore = null;

        base.OnLeave(fsm, isShutdown);
    }

    /// <summary>
    /// 检查是否能够设置连击
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    protected virtual bool CheckCanSetCombo(int skillId)
    {
        return SkillDataCore.IsComboGroupSkill(skillId);
    }

    public override bool CheckCanSkill(int skillID)
    {
        //当前是连击技能
        if (_curSkillIsComboGroupSkill)
        {
            bool isComboGroup = SkillDataCore.IsComboGroupSkill(skillID);
            if (!isComboGroup)//后续主动是主动技能直接打断
            {
                return true;
            }
            else//后序还是连击技能需要判断是否能够打断
            {
                if (CheckCanSetCombo(skillID))
                {
                    return true;
                }
            }
        }

        return base.CheckCanSkill(skillID);
    }
}