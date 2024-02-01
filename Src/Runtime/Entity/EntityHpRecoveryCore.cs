/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体脱战回血组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityHpRecoveryCore.cs
 * 
 */


/// <summary>
/// 实体脱战回血组件
/// </summary>
public class EntityHpRecoveryCore : EntityBaseComponent
{
    public bool IsHpRecovery { get; private set; }
    private void Start()
    {
        RefEntity.EntityEvent.ChangeIsBattle += OnChangeIsBattle;
        RefEntity.EntityEvent.EnterDeath += OnEnterDeath;
        RefEntity.EntityEvent.EntityBeReborn += OnEntityBeReborn;
        CheckHpRecovery();
    }

    private void OnDestroy()
    {
        StopHpRecovery();
        if (RefEntity != null)
        {
            RefEntity.EntityEvent.ChangeIsBattle -= OnChangeIsBattle;
            RefEntity.EntityEvent.EnterDeath -= OnEnterDeath;
            RefEntity.EntityEvent.EntityBeReborn += OnEntityBeReborn;
        }
    }
    private void OnChangeIsBattle(bool isBattle)
    {
        CheckHpRecovery();
    }

    private void OnEnterDeath()
    {
        CheckHpRecovery();
    }

    private void OnEntityBeReborn()
    {
        CheckHpRecovery();
    }

    private void CheckHpRecovery()
    {
        if (!RefEntity.BattleDataCore.IsInBattle && RefEntity.BattleDataCore.IsLive())
        {
            StartHpRecovery();
        }
        else
        {
            StopHpRecovery();
        }
    }
    private void StartHpRecovery()
    {
        if (IsHpRecovery)
        {
            return;
        }
        IsHpRecovery = true;
        TimerMgr.AddTimer(GetHashCode(), TimeUtil.S2MS, OnHpRecovery, 0);
    }

    private void StopHpRecovery()
    {
        if (!IsHpRecovery)
        {
            return;
        }
        IsHpRecovery = false;
        _ = TimerMgr.RemoveTimer(GetHashCode());
    }
    protected virtual void OnHpRecovery()
    {
        if (!IsHpRecovery)
        {
            return;
        }

        int hp = RefEntity.BattleDataCore.HP + RefEntity.BattleDataCore.HPRecovery;
        RefEntity.BattleDataCore.SetHP(hp);
    }
}