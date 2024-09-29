/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 添加实体技能效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEAddEntityCore.cs
* 
*/

using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

public class SEAddEntityCore : SkillEffectBase
{
    protected List<ResourcesPointData> PointDataList = new();
    public override void OnAdd()
    {
        base.OnAdd();
    }

    public override void OnRemove()
    {
        PointDataList.Clear();
        base.OnRemove();
    }

    public override DamageEffect CreateEffectData(EntityBase fromEntity, EntityBase targetEntity, InputSkillReleaseData inputData)
    {
        DamageEffect effect = new();
        if (EffectCfg.Parameters2 == null || EffectCfg.Parameters2.Length <= 0)
        {
            Log.Error($"SEAddEntityCore Parameters Error EffectID = {EffectID}");
            return null;
        }

        UnityEngine.Vector3 targetPos = targetEntity.Position;
        if (inputData.DRSkill.IsRemote && inputData.TargetPosList != null && inputData.TargetPosList.Length > 0)
        {
            targetPos = inputData.TargetPosList[0];
        }

        //实体类型,实体ID,刷新间隔,刷新数量,刷新半径,最小等级,最大等级
        for (int i = 0; i < EffectCfg.Parameters2.Length; i++)
        {
            if (EffectCfg.Parameters2[i].Length < 7)
            {
                Log.Error($"SEAddEntityCore Parameters2 Error EffectID = {EffectID}");
                continue;
            }

            SummonResourcesPointData resourcesPointData = new();
            resourcesPointData.ResourceType = EffectCfg.Parameters2[i][0];
            resourcesPointData.ConfigId = EffectCfg.Parameters2[i][1];
            resourcesPointData.UpdateInterval = EffectCfg.Parameters2[i][2];
            resourcesPointData.UpdateNum = EffectCfg.Parameters2[i][3];
            resourcesPointData.Radius = EffectCfg.Parameters2[i][4];

            resourcesPointData.LevelRange = new float[2];
            resourcesPointData.LevelRange[0] = EffectCfg.Parameters2[i][5];
            resourcesPointData.LevelRange[1] = EffectCfg.Parameters2[i][6];

            resourcesPointData.X = targetPos.x;
            resourcesPointData.Y = targetPos.y;
            resourcesPointData.Z = targetPos.z;
            resourcesPointData.FromID = fromEntity.BaseData.Id;
            PointDataList.Add(resourcesPointData);
        }
        return effect;
    }
}