/*
 * @Author: xiang huan
 * @Date: 2022-08-26 14:25:46
 * @Description: 实体碰撞盒
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityCollisionCore.cs
 * 
 */
using CMF;
using UnityEngine;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using System;

public abstract class EntityCollisionCore : EntityBaseComponent
{
    /// <summary>
    /// 碰撞盒所在GameObject 未加载完成为null
    /// </summary>
    /// <value></value>
    public GameObject CollisionObject { get; private set; }
    /// <summary>
    /// 碰撞盒 未加载完成为null 可以拿到碰撞盒的bounds范围盒中心点
    /// </summary>
    /// <value></value>
    public Collider BodyCollision { get; private set; }  //躯干碰撞盒

    public EntityCollisionTriggerCore EntityTrigger { get; private set; } //实体触发器
    public HashSet<long> EntityTriggerSet => EntityTrigger == null ? null : EntityTrigger.EntityTriggerSet; //触碰实体Map
    public int EntityTriggerActiveCount = 0; //实体触发器激活计数
    /// <summary>
    /// 加载碰撞预制体
    /// </summary>
    public abstract void LoadCollision(string assetPath);

    /// <summary>
    /// 卸载碰撞预制体
    /// </summary>
    public virtual void UnloadCollision()
    {
        if (CollisionObject == null)
        {
            return;
        }
        //Destroy(CollisionObject);
        CollisionObject = null;
        BodyCollision = null;
    }

    /// <summary>
    /// 初始化碰撞盒
    /// </summary>
    public virtual void Init(GameObject prefab)
    {
        if (prefab == null)
        {
            Log.Error("EntityCollision prefab is Null");
            return;
        }

        //因为现在移动主要都是依赖角色控制器 但是角色控制器控制的移动只能是控制器所在对象 所以只能将预制件中的碰撞器参数复制到实体根上来移动 这是暂时折中办法
        if (prefab.TryGetComponent(out Mover prefabMover))
        {
            RefEntity.EntityRoot.layer = prefab.layer;//暂时直接直接赋值成碰撞盒的层

            //先手动创建移动碰撞的依赖组件
            _ = RefEntity.AddComponent<Rigidbody>();

            Collider prefabCollider = prefab.GetComponent<Collider>();
            CapsuleCollider collider = RefEntity.AddComponent<CapsuleCollider>();
            collider.isTrigger = prefabCollider.isTrigger;

            Mover mover = RefEntity.AddComponent<Mover>();
            //将预制件中预览的碰撞盒参数反向设置到新mover中 这样可以可视化调整collider而无需去手动设置mover参数
            SetMoverDataFromCollider(mover, prefabCollider);
            // mover.SetColliderHeight(prefabMover.ColliderHeight);
            // mover.SetColliderThickness(prefabMover.ColliderThickness);
            // mover.SetColliderOffset(prefabMover.ColliderOffset);
            // mover.SetStepHeightRatio(MoveDefine.MOVE_STEP_HEIGHT_RATIO);

            CollisionObject = collider.gameObject;
            BodyCollision = collider;

            AddEntityTrigger();
        }
        else
        {
            // 静态实体
            CollisionObject = GameObject.Instantiate(prefab);
            CollisionObject.transform.parent = RefEntity.GetTransform();
            CollisionObject.transform.localPosition = Vector3.zero;
            CollisionObject.transform.localRotation = Quaternion.identity;
            BodyCollision = CollisionObject.GetComponent<Collider>();
        }

        EntityReferenceData refData = CollisionObject.AddComponent<EntityReferenceData>();
        refData.SetEntity(RefEntity);

        if (RefEntity.TryGetComponent(out RoleBaseDataCore roleData))
        {
            roleData.SetHeight(prefabMover.ColliderHeight);
            roleData.SetRadius(prefabMover.ColliderThickness * 0.5f);
        }
        RefEntity.EntityEvent.ColliderLoadFinish?.Invoke(CollisionObject);
    }

    /// <summary>
    /// 将想要的碰撞盒的模样的参数反向设置回Mover中
    /// </summary>
    /// <param name="mover"></param>
    /// <param name="collider"></param>
    private void SetMoverDataFromCollider(Mover mover, Collider collider)
    {
        if (mover == null || collider == null)
        {
            return;
        }

        if (collider is CapsuleCollider capsuleCollider)
        {
            mover.SetColliderHeight(capsuleCollider.height);
            mover.SetColliderThickness(capsuleCollider.radius * 2);
            mover.SetColliderOffset(capsuleCollider.center / capsuleCollider.height);
        }
        else if (collider is SphereCollider sphereCollider)
        {
            float moverHeight = sphereCollider.radius * 2;
            mover.SetColliderHeight(moverHeight);
            mover.SetColliderThickness(moverHeight);
            mover.SetColliderOffset(sphereCollider.center / moverHeight);
        }
        else
        {
            Log.Error("SetMoverDataFromCollider collider type not support");
            mover.SetColliderHeight(collider.bounds.size.y);
            mover.SetColliderThickness(collider.bounds.size.x);
            mover.SetColliderOffset(collider.bounds.center / collider.bounds.size.y);
        }

        mover.SetStepHeightRatio(MoveDefine.MOVE_STEP_HEIGHT_RATIO);
    }

    /// <summary>
    /// 添加实体触发器
    /// </summary>
    public void AddEntityTrigger()
    {
        RemoveEntityTrigger();
        GameObject entityTriggerObject = new();
        entityTriggerObject.transform.parent = transform;
        entityTriggerObject.transform.localPosition = Vector3.zero;
        entityTriggerObject.transform.localRotation = Quaternion.identity;
        entityTriggerObject.layer = MLayerMask.ENTITY_CHECK;
        entityTriggerObject.name = "EntityCheck";

        CapsuleCollider bodyCollider = BodyCollision as CapsuleCollider;
        CapsuleCollider collider = entityTriggerObject.AddComponent<CapsuleCollider>();
        collider.radius = bodyCollider.radius;
        collider.height = bodyCollider.height;
        collider.center = bodyCollider.center;
        collider.isTrigger = true;

        EntityTrigger = entityTriggerObject.AddComponent<EntityCollisionTriggerCore>();
        EntityTrigger.Init(RefEntity);
        entityTriggerObject.SetActive(EntityTriggerActiveCount > 0);
    }
    /// <summary>
    /// 删除实体触发器
    /// </summary>
    public void RemoveEntityTrigger()
    {
        if (EntityTrigger != null)
        {
            Destroy(EntityTrigger.gameObject);
            EntityTrigger = null;
        }
    }

    public void SetEntityTriggerActive(bool active)
    {
        EntityTriggerActiveCount = active ? EntityTriggerActiveCount + 1 : EntityTriggerActiveCount - 1;

        if (EntityTrigger == null)
        {
            return;
        }
        EntityTrigger.gameObject.SetActive(EntityTriggerActiveCount > 0);

        if (EntityTriggerActiveCount <= 0)
        {
            EntityTrigger.Clear();
        }
    }

    private void OnDestroy()
    {
        RemoveEntityTrigger();
        UnloadCollision();
    }
}