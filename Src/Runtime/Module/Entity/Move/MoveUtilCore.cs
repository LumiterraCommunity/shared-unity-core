using GameMessageCore;
using UnityGameFramework.Runtime;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 移动工具类
/// </summary>
public static class MoveUtilCore
{
    /// <summary>
    /// 获取实体的网络移动数据 用于网络同步 参数给定可以节省GC
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dest">目标位置 为空时代表停止没有目的</param>
    /// <param name="movement">给定会节省GC 返回本对象</param>
    /// <returns></returns>
    public static EntityMovement GetEntityNetMovement(EntityBase entity, Vector3? dest, EntityMovement movement)
    {
        movement ??= new EntityMovement();

        movement.TypeId = new EntityId()
        {
            Id = entity.BaseData.Id,
            Type = entity.BaseData.Type
        };

        movement.CurLocation = new EntityMoveStep()
        {
            Location = NetUtilCore.LocToNet(entity.Position),
            Stamp = TimeUtil.GetServerTimeStamp()
        };
        movement.DestLocation = dest == null ? null : NetUtilCore.LocToNet(dest.Value);

        movement.Type = dest == null ? MovementType.Idle : MovementType.Run;
        movement.Dir = NetUtilCore.DirToNet(entity.Forward);
        movement.MoveSpeed = entity.MoveData.Speed;

        if (entity.TryGetComponent(out CharacterMoveCtrl moveCtrl))
        {
            movement.CurFinallySpeed = NetUtilCore.Vector3ToNet(moveCtrl.CurSpeed);
        }
        else
        {
            movement.CurFinallySpeed = new GameMessageCore.Vector3();
            Log.Error($"GetEntityNetMovement CharacterMoveCtrl is null,entityType:{entity.BaseData.Type}");
        }

        return movement;
    }

    /// <summary>
    /// 强行应用网络移动数据恢复当前移动状态 不会更改业务状态 仅仅处理移动底层数据
    /// 相当于完全还原后端状态 还会设置当前即时速度 因为场景中的modify都还存在只需要更新下即时速度即可和后端完全同步
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="movement"></param>
    public static void ForceApplyNetMovement(EntityBase entity, EntityMovement movement)
    {
        entity.SetPosition(NetUtilCore.Vector3FromNet(movement.CurLocation.Location.Loc));
        entity.MoveData.SetMoveSpeed(movement.MoveSpeed);
        if (entity.TryGetComponent(out CharacterMoveCtrl moveCtrl))
        {
            moveCtrl.SetCurSpeed(NetUtilCore.Vector3FromNet(movement.CurFinallySpeed));
        }
    }
}