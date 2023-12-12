/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 添加实体技能效果
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEAuraCore.cs
* 
*/

using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SEAuraCore : SkillEffectBase
{
    protected ListMap<long, EntityBase> EntityList = new();
    protected HashSet<EntityBase> SearchEntityMap = new();
#if UNITY_EDITOR
    private GameObject _drawObj;
#endif
    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        //同步效果球时，在前端执行是不会同步技能输入数据的，所以这里需要手动创建
        InputSkillReleaseData inputData = new(SkillID, RefEntity.Forward);
        SetInputData(inputData);

#if UNITY_EDITOR
        _drawObj = new GameObject("SEAuraCore");
        SkillShapeGizmos shapeGizmos = _drawObj.AddComponent<SkillShapeGizmos>();

        UnityEngine.Vector3 targetPos = new(EffectData.PosValue.X, EffectData.PosValue.Y, EffectData.PosValue.Z);
        if (inputData.DRSkill.SkillRange != null && inputData.DRSkill.SkillRange.Length > 0)
        {
            shapeGizmos.StartDraw(inputData.DRSkill.SkillRange, targetPos, inputData.Dir);
        }
#endif
    }
    public override void OnAdd()
    {
        base.OnAdd();
    }

    public override void OnRemove()
    {
#if UNITY_EDITOR
        UnityEngine.Object.Destroy(_drawObj);
#endif
        RemoveAllEntity();
        SearchEntityMap.Clear();
        base.OnRemove();
    }

    public override void OnInterval()
    {
        base.OnInterval();
        if (EffectData == null)
        {
            return;
        }

        if (EffectCfg.Parameters == null || EffectCfg.Parameters.Length <= 0)
        {
            Log.Error($"SEAuraCore Parameters Error EffectID = {EffectID}");
            return;
        }

        SearchEntityMap.Clear();
        UnityEngine.Vector3 targetPos = new(EffectData.PosValue.X, EffectData.PosValue.Y, EffectData.PosValue.Z);
        SkillUtil.SearchTargetEntityMap(targetPos, RefEntity, SearchEntityMap, InputData.DRSkill.SkillRange, InputData.Dir, InputData.TargetType);
        //移除已经不存在的实体
        if (EntityList.Count > 0)
        {
            for (int i = EntityList.Count - 1; i >= 0; i--)
            {
                if (SearchEntityMap.Contains(EntityList[i]))
                {
                    continue;
                }
                RemoveEntity(EntityList[i]);
            }
        }

        //添加新的实体
        if (SearchEntityMap.Count > 0)
        {
            foreach (EntityBase entity in SearchEntityMap)
            {
                if (EntityList.ContainsKey(entity.BaseData.Id))
                {
                    continue;
                }

                AddEntity(entity);
            }
        }
    }

    protected void AddEntity(EntityBase entity)
    {
        if (EntityList.ContainsKey(entity.BaseData.Id))
        {
            return;
        }
        entity.EntityEvent.UnInitFromScene += UnInitFromScene;
        _ = SkillUtil.EntitySkillEffectExecute(InputData, EffectCfg.Parameters, RefEntity, entity);
        _ = EntityList.Add(entity.BaseData.Id, entity);
    }

    protected void RemoveEntity(EntityBase entity)
    {
        if (!EntityList.ContainsKey(entity.BaseData.Id))
        {
            return;
        }
        entity.EntityEvent.UnInitFromScene -= UnInitFromScene;
        SkillUtil.EntityAbolishSkillEffect(SkillID, EffectCfg.Parameters, RefEntity, entity);
        _ = EntityList.Remove(entity.BaseData.Id);
    }

    protected void RemoveAllEntity()
    {
        if (EntityList.Count <= 0)
        {
            return;
        }
        for (int i = EntityList.Count - 1; i >= 0; i--)
        {
            EntityList[i].EntityEvent.UnInitFromScene -= UnInitFromScene;
            SkillUtil.EntityAbolishSkillEffect(SkillID, EffectCfg.Parameters, RefEntity, EntityList[i]);
        }
        EntityList.Clear();
    }

    protected virtual void UnInitFromScene(EntityBase entity)
    {
        RemoveEntity(entity);
    }
    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        DamageEffect effect = new();
        UnityEngine.Vector3 targetPos = targetEntity.Position;
        if (inputData.DRSkill.IsRemote && inputData.TargetPosList != null && inputData.TargetPosList.Length > 0)
        {
            targetPos = inputData.TargetPosList[0];
        }
        effect.PosValue = new()
        {
            X = targetPos.x,
            Y = targetPos.y,
            Z = targetPos.z
        };
        return effect;
    }
}