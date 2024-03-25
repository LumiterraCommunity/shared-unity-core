/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 添加实体技能效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEAuraCore.cs
* 
*/

using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

public class SEAuraCore : SkillEffectBase
{
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

#if UNITY_EDITOR
        _drawObj = new GameObject("SEAuraCore");
        SkillShapeGizmos shapeGizmos = _drawObj.AddComponent<SkillShapeGizmos>();

        UnityEngine.Vector3 targetPos = new(EffectData.PosValue.X, EffectData.PosValue.Y, EffectData.PosValue.Z);
        if (InputData.DRSkill.SkillRange != null && InputData.DRSkill.SkillRange.Length > 0)
        {
            shapeGizmos.StartDraw(InputData.DRSkill.SkillRange, targetPos, InputData.Dir);
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

        if (SearchEntityMap.Count > 0)
        {
            foreach (EntityBase entity in SearchEntityMap)
            {
                _ = SkillUtil.EntitySkillEffectExecute(InputData, EffectCfg.Parameters, RefEntity, entity);
            }
        }
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