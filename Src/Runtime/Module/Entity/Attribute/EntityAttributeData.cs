/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体属性数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/EntityAttributeData.cs
 * 
 */
using UnityGameFramework.Runtime;

public class EntityAttributeData : AttributeDataCpt, IEntityComponent
{
    private EntityBase _refEntity;
    /// <summary>
    /// 获取组件所挂载的实体的反引用
    /// </summary>
    /// <value></value>
    public EntityBase RefEntity
    {
        get
        {
            if (_refEntity == null)
            {
                _refEntity = GFEntryCore.GetModule<IEntityMgr>().GetEntityWithRoot<EntityBase>(gameObject);
            }

            return _refEntity;
        }
    }

    public void InitEntity(EntityBase entity)
    {
        _refEntity = entity;
    }

    protected override void OnAttributeUpdate(eAttributeType type, IntAttribute attribute)
    {
        base.OnAttributeUpdate(type, attribute);

        try
        {
            RefEntity.EntityEvent.EntityAttributeUpdate?.Invoke(type, attribute.Value);
        }
        catch (System.Exception e)
        {
            Log.Error($"EntityAttributeUpdate Error: {e}");
        }
    }
}