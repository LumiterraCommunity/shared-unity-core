using UnityGameFramework.Runtime;
/// <summary>
/// 实体上的各种状态的基类 实体状态类直接可以在内部任意时间切换状态 不要求再update 切换时取OwnerFsm
/// </summary>
public class EntityStatusCore : ComponentStatusCore<EntityStatusCtrl>
{
    protected virtual bool RefEntityIsDead()
    {
        try
        {
            //因为流程是先进入死亡状态，再进入灵魂状态，所以这里判断是否死亡，是根据IsLive判断的
            return StatusCtrl.RefEntity != null && StatusCtrl.RefEntity.BattleDataCore != null && !StatusCtrl.RefEntity.BattleDataCore.IsLive();
        }
        catch (System.Exception e)
        {
            Log.Error($"check entity is dead error {e}");
            return false;
        }

    }

    protected virtual bool RefEntityIsSoul()
    {
        return StatusCtrl.RefEntity.BattleDataCore != null && StatusCtrl.RefEntity.BattleDataCore.IsInSoul;
    }
}