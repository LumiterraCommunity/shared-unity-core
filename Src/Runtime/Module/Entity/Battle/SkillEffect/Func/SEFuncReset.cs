/*
 * @Author: xiang huan
 * @Date: 2023-07-26 16:01:51
 * @Description: 检测重置层级
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFunReset.cs
 * 
 */


using UnityGameFramework.Runtime;

public class SEFuncReset : SEFuncUpdateLayer
{
    public override void Init()
    {
        base.Init();
        //
    }
    public override void Clear()
    {
        //
        base.Clear();
    }
    protected override void OnAddEvent()
    {
        base.OnAddEvent();
        RefEntity.EntityEvent.ChangeIsBattle += OnChangeIsBattle;
    }

    protected override void OnRemoveEvent()
    {
        RefEntity.EntityEvent.ChangeIsBattle -= OnChangeIsBattle;
        base.OnRemoveEvent();
    }

    private void OnChangeIsBattle(bool isBattle)
    {
        if (!isBattle)
        {
            UpdateLayer(1);
        }
    }
}