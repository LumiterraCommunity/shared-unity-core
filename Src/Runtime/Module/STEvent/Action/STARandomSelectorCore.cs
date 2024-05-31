/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 根据配置权重随机选择执行一个子行为
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Action/STARandomSelectorCore.cs
 * 
 */

using System.Collections.Generic;
/// <summary>
/// 根据配置权重随机选择执行一个子行为
/// DRSceneEventAction.Parameters_2: [[权重,子行为CID]...]
/// </summary>
public class STARandomSelectorCore : STActionBase
{
    protected List<STActionBase> ChildActions = new();  //子行为列表
    protected List<int> WeightList = new(); //权重列表
    public override void Init(DRSceneEventAction cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        InitChildActions();
    }

    private void InitChildActions()
    {

        for (int i = 0; i < DRSceneEventAction.Parameters_2.Length; i++)
        {
            int weight = DRSceneEventAction.Parameters_2[i][0];
            int actionCid = DRSceneEventAction.Parameters_2[i][1];
            STActionBase action = GFEntryCore.SceneTriggerEventMgr.CreateSTAction(actionCid, SceneEvent);
            ChildActions.Add(action);
            WeightList.Add(weight);
        }
    }

    public override void Execute()
    {
        int index = MathUtilCore.RandomWeightListIndex(WeightList);
        if (index >= 0 && index < ChildActions.Count)
        {
            ChildActions[index].Execute();
        }
    }

    public override void Clear()
    {
        ClearChildActions();
        WeightList.Clear();
        base.Clear();
    }

    private void ClearChildActions()
    {
        if (ChildActions != null)
        {
            for (int i = 0; i < ChildActions.Count; i++)
            {
                ChildActions[i].Dispose();
            }
            ChildActions.Clear();
        }
    }
}