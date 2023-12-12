/** 
 * @Author XQ
 * @Date 2022-08-11 20:12:50
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/EventFunction/IStatusEventFunction.cs
 */

using GameFramework;

/// <summary>
/// 状态上用来监听实体事件的一些通用功能的接口
/// </summary>
public interface IStatusEventFunction : IReference
{
    void AddEvent(EntityEvent entityEvent);
    void RemoveEvent(EntityEvent entityEvent);
}