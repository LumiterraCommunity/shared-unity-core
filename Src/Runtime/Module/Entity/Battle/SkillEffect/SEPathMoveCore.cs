/*
 * @Author: xiang huan
 * @Date: 2022-08-12 14:36:36
 * @Description: 技能路径移动效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEPathMoveCore.cs
 * 
 */


using UnityEngine;
using UnityGameFramework.Runtime;

public class SEPathMoveCore : SkillEffectBase
{
    protected CharacterMoveCtrl CharacterMoveCtrl;
    protected MoveModifier MoveModifier;
    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        if (!RefEntity.TryGetComponent(out CharacterMoveCtrl))
        {
            Log.Error($"SEPathMoveCore not find CharacterMoveCtrl cpt");
            return;
        }
        if (RefEntity.EntityCheckPosCore != null)
        {
            float distance = EffectCfg.Parameters[0] * MathUtilCore.CM2M;
            RefEntity.EntityCheckPosCore.AddErrorDist(distance);
        }
        TimerMgr.AddTimer(GetHashCode(), EffectData.BeatBackValue.DelayTime, Move);
    }

    //移动
    private void Move()
    {
        RefEntity.GetComponent<EntityEvent>().SpecialMoveStartNotMoveStatus?.Invoke();
        //实际位置
        Vector3 realPos = RefEntity.Position;
        Vector3 curLoc = NetUtilCore.LocFromNet(EffectData.BeatBackValue.CurLoc);
        Vector3 targetPos = NetUtilCore.LocFromNet(EffectData.BeatBackValue.BackToPos);
        // 检测从当前位置和预计位置是否有效，无效则走预计位置
        if (RefEntity.EntityCheckPosCore != null && !RefEntity.EntityCheckPosCore.CheckPositionValid(realPos, curLoc))
        {
            realPos = NetUtilCore.LocFromNet(EffectData.BeatBackValue.CurLoc);
        }
        Vector3 offset = targetPos - realPos;
        float distance = offset.magnitude;
        if (distance.ApproximatelyEquals(0) || offset.ApproximatelyEquals(Vector3.zero))
        {
            return;
        }
        float speed = distance / ((EffectCfg.Duration - EffectData.BeatBackValue.DelayTime) * TimeUtil.MS2S);
        Vector3 moveSpeed = offset.normalized * speed;
        AddMoveModifier(moveSpeed);
        Vector3 curSpeed = new(moveSpeed.x, 0, moveSpeed.z);
        CharacterMoveCtrl.AddCurSpeedDelta(curSpeed);
    }
    public override void OnRemove()
    {
        RemoveMoveModifier();
        CharacterMoveCtrl = null;
        _ = TimerMgr.RemoveTimer(GetHashCode());
    }

    public override GameMessageCore.DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        if (EffectCfg.Parameters == null || EffectCfg.Parameters.Length <= 0)
        {
            Log.Error($"SEPathMove Parameters Error EffectID = {EffectID}");
            return null;
        }
        float distance = EffectCfg.Parameters[0] * MathUtilCore.CM2M;
        int delayTime = 0;
        if (EffectCfg.Parameters.Length > 1)
        {
            delayTime = EffectCfg.Parameters[1];
        }

        int dir = 1;
        if (EffectCfg.Parameters.Length > 2)
        {
            dir = EffectCfg.Parameters[2];
        }

        float y = 0;
        if (EffectCfg.Parameters.Length > 3)
        {
            y = EffectCfg.Parameters[3];
        }

        Vector3 curPos = targetEntity.Position;
        Vector3 forward = targetEntity.Forward;
        forward.Set(forward.x * dir, y, forward.z * dir);
        Vector3 targetPos = curPos + (forward.normalized * distance);
        GameMessageCore.DamageEffect effect = new();
        effect.BeatBackValue = new();
        effect.BeatBackValue.CurLoc = NetUtilCore.LocToNet(curPos);
        effect.BeatBackValue.BackToPos = NetUtilCore.LocToNet(targetPos);
        effect.BeatBackValue.DelayTime = delayTime;
        return effect;
    }

    /// <summary>
    /// 添加移动速度修改器
    /// </summary>
    private void AddMoveModifier(Vector3 speed)
    {
        RemoveMoveModifier();
        MoveModifier = CharacterMoveCtrl.AddMove(speed, Vector3.zero);
    }

    /// <summary>
    /// 移除移动速度修改器
    /// </summary>
    private void RemoveMoveModifier()
    {
        if (MoveModifier != null)
        {
            CharacterMoveCtrl.RemoveMove(MoveModifier);
            MoveModifier = null;
        }
    }
}
