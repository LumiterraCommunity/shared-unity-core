/** 
 * @Author XQ
 * @Date 2022-08-09 10:25:20
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Move/DirectionMove.cs
 */
using UnityEngine;

/// <summary>
/// 执行方向移动 拿到方向 每帧执行 需要子类来驱动具体执行
/// </summary>
[RequireComponent(typeof(EntityInputData))]
public abstract class DirectionMove : EntityMoveBase
{
    protected EntityInputData InputData;

    protected virtual void Start()
    {
        InputData = GetComponent<EntityInputData>();
    }
}