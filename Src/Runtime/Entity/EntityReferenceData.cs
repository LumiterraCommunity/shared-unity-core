/*
 * @Author: xiang huan
 * @Date: 2022-08-16 11:26:50
 * @Description: 用于实体与节点建立引用关系
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityReferenceData.cs
 * 
 */

using UnityEngine;
public class EntityReferenceData : MonoBehaviour
{
    public EntityBase Entity { get; private set; }

    public void SetEntity(EntityBase entity)
    {
        Entity = entity;
    }
}