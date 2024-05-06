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
    /// <param name="refEntity"></param>
    /// <param name="dest">目标位置 为空时代表停止没有目的</param>
    /// <param name="movement">给定会节省GC 返回本对象</param>
    /// <returns></returns>
    public static EntityMovement GetEntityNetMovement(EntityBase refEntity, Vector3? dest, EntityMovement movement)
    {
        movement ??= new EntityMovement();

        movement.TypeId = new EntityId()
        {
            Id = refEntity.BaseData.Id,
            Type = refEntity.BaseData.Type
        };

        movement.CurLocation = new EntityMoveStep()
        {
            Location = NetUtilCore.LocToNet(refEntity.Position),
            Stamp = TimeUtil.GetServerTimeStamp()
        };
        movement.DestLocation = dest == null ? null : NetUtilCore.LocToNet(dest.Value);

        movement.Type = dest == null ? MovementType.Idle : MovementType.Run;
        movement.Dir = NetUtilCore.DirToNet(refEntity.Forward);
        movement.MoveSpeed = refEntity.MoveData.Speed;

        if (refEntity.TryGetComponent(out CharacterMoveCtrl moveCtrl))
        {
            movement.CurFinallySpeed = NetUtilCore.Vector3ToNet(moveCtrl.CurSpeed);
        }
        else
        {
            movement.CurFinallySpeed = new GameMessageCore.Vector3();
            Log.Error($"GetEntityNetMovement CharacterMoveCtrl is null,entityType:{refEntity.BaseData.Type}");
        }

        return movement;
    }
}