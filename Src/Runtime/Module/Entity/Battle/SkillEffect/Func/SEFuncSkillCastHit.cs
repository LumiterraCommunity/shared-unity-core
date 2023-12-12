/*
 * @Author: xiang huan
 * @Date: 2023-07-26 16:01:51
 * @Description: 检测技能命中，更新层级
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFuncSkillCastHit.cs
 * 
 */


using UnityGameFramework.Runtime;

public class SEFuncSkillCastHit : SEFuncUpdateLayer
{

    protected int SkillID; //技能ID, -1为任意技能
    protected int AddLayerNum; //增加层级数
    public override void Init()
    {
        base.Init();
        int[] parameters = (int[])FuncData;
        if (parameters == null || parameters.Length != 2)
        {
            Log.Error($"SEFuncSkillCastHit FuncData error SkillEffectID = {SkillEffect.EffectID} ");
            return;
        }
        SkillID = parameters[0];
        AddLayerNum = parameters[1];
    }
    public override void Clear()
    {
        SkillID = -1;
        AddLayerNum = 0;
        base.Clear();
    }
    protected override void OnAddEvent()
    {
        base.OnAddEvent();
        RefEntity.EntityEvent.SkillCastHit += OnSkillCastHit;
    }

    protected override void OnRemoveEvent()
    {
        RefEntity.EntityEvent.SkillCastHit -= OnSkillCastHit;
        base.OnRemoveEvent();
    }

    private void OnSkillCastHit(InputSkillReleaseData inputSkillReleaseData)
    {
        if (SkillID != -1 && inputSkillReleaseData.SkillID != SkillID)
        {
            return;
        }
        UpdateLayer(AddLayerNum + SkillEffect.CurLayer);
    }

}