using System;
using UnityEngine;

/// <summary>
/// 技能飞行物
/// </summary>
public abstract class SkillFlyerCore : MonoBehaviour
{
    public int FlyerID { get; private set; }
    public long FormEntityID { get; private set; }
    public DRSkill DRSkill { get; private set; }
    public DRSkillFlyer DRSkillFlyer { get; private set; }
    public long TargetEntityID { get; private set; }//没有为 BattleDefine.ENTITY_ID_UNKNOWN
    public Vector3? FlyEndPos { get; private set; }//没有为Null

    public void Init(int flyerID, long formEntityID, DRSkill drSkill)
    {
        FlyerID = flyerID;
        FormEntityID = formEntityID;
        DRSkill = drSkill;
        DRSkillFlyer = GFEntryCore.DataTable.GetDataTable<DRSkillFlyer>().GetDataRow(drSkill.SkillFlyerId);
        OnInit();
    }

    public void SetTargetEntityID(long targetID)
    {
        TargetEntityID = targetID;
        FlyEndPos = null;
        OnStart();
    }

    public void SetFlyEndPos(Vector3 pos)
    {
        FlyEndPos = pos;
        TargetEntityID = BattleDefine.ENTITY_ID_UNKNOWN;
        OnStart();
    }

    /// <summary>
    /// 移动到达时事件回调 子类实现
    /// </summary>
    public abstract void OnMoveArrived(object sender, EventArgs e);

    /// <summary>
    /// 初始化完成 子类可选实现
    /// </summary>
    protected virtual void OnInit() { }

    /// <summary>
    /// 开始 子类可选实现
    /// </summary>
    protected virtual void OnStart() { }
}