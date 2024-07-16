using System.Linq;
using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 角色天赋数据
/// </summary>
public class PlayerTalentTreeDataCore : EntityBaseComponent
{
    /// <summary>
    /// 天赋树等级列表
    /// </summary>
    /// <returns></returns>
    protected List<TalentLevel> TalentTreeLvList;
    /// <summary>
    /// 所有类型的天赋树列表，方便遍历
    /// </summary>
    protected Dictionary<TalentType, List<TalentNode>> AllTalentTreeList;
    /// <summary>
    /// 所有类型的天赋树字典，方便查找
    /// </summary>
    protected Dictionary<TalentType, Dictionary<int, TalentNode>> AllTalentTreeDic;
    /// <summary>
    /// 天赋树配置表引用
    /// </summary>
    private GameFramework.DataTable.IDataTable<DRTalentTree> _refTalentTreeTable;

    private void Awake()
    {
        _refTalentTreeTable = GFEntryCore.DataTable.GetDataTable<DRTalentTree>();
    }

    private void OnDestroy()
    {
        AllTalentTreeList = null;
        AllTalentTreeDic = null;
        _refTalentTreeTable = null;
    }

    /// <summary>
    /// 初始化技能树数据
    /// 后面的变动通过 UpdateNode,AddNode,RemoveNode 来更新
    /// </summary>
    /// <param name="talentData"></param>
    public bool Init(RpcTalentData talentData)
    {
        Log.Info($"player:{RefEntity.BaseData.Id} start init talent tree");
        AllTalentTreeList = new();
        AllTalentTreeDic = new();
        TalentTreeLvList = new();

        if (talentData.Levels != null)
        {
            TalentTreeLvList.AddRange(talentData.Levels);//天赋树等级列表
        }

        if (talentData.Trees == null)
        {
            RefEntity.EntityEvent.TalentSkillInited?.Invoke(new List<int>());
            //整棵树为空，直接返回成功
            return true;
        }

        try
        {
            int treeCount = talentData.Trees.Count;
            for (int treeIndex = 0; treeIndex < treeCount; treeIndex++)
            {
                TalentTree tree = talentData.Trees[treeIndex];
                List<TalentNode> treeList = new();
                Dictionary<int, TalentNode> treeDic = new();
                AllTalentTreeList.Add(tree.Type, treeList);
                AllTalentTreeDic.Add(tree.Type, treeDic);

                if (tree.UnlockNodes != null)
                {
                    treeList.AddRange(tree.UnlockNodes);
                    for (int i = 0; i < tree.UnlockNodes.Count; i++)
                    {
                        TalentNode node = tree.UnlockNodes[i];
                        treeDic.Add(node.NodeId, node);
                    }
                }
            }

            RefEntity.EntityEvent.TalentSkillInited?.Invoke(GetTalentSkills());
            Log.Info("init talent tree success");
            return true;
        }
        catch (System.Exception e)
        {
            Log.Error($"init talent tree success,but some error happened, error: {e.Message}");
            RefEntity.EntityEvent.TalentSkillInited?.Invoke(new List<int>());
            return false;
        }
    }

    /// <summary>
    /// 获取天赋收益
    /// </summary>
    /// <param name="gainsType">收益类型</param>
    /// <param name="talentType">天赋类型</param>
    /// <returns></returns>
    public List<int> GetTalentGains(TalentGainsType gainsType, TalentType talentType = TalentType.Unknown)
    {
        List<int> result = new();
        if (talentType == TalentType.Unknown)
        {
            foreach (TalentType type in System.Enum.GetValues(typeof(TalentType)))
            {
                if (type != TalentType.Unknown)
                {
                    result.AddRange(GetTalentGains(gainsType, type));
                }
            }

            return result;
        }

        if (!AllTalentTreeList.TryGetValue(talentType, out List<TalentNode> treeList))
        {
            return result;
        }

        if (treeList == null || treeList.Count == 0)
        {
            Log.Warning($"can not find talent tree, talent type: {talentType}");
            return result;
        }

        for (int i = 0; i < treeList.Count; i++)
        {
            TalentNode node = treeList[i];
            if (node.Level <= 0)
            {
                //虽然节点解锁了，但是等级还是0，没有收益，不必计算
                continue;
            }

            DRTalentTree nodeCfg = _refTalentTreeTable.GetDataRow(node.NodeId);
            if (nodeCfg == null)
            {
                Log.Error($"can not find talent tree config, node id: {node.NodeId}");
                continue;
            }

            List<int> gainsList = TableUtil.GetTalentNodeGains(nodeCfg, node.Level, (int)gainsType);
            if (gainsList != null && gainsList.Count > 0)
            {
                result.AddRange(gainsList);
            }
        }

        return result;
    }

    public List<int> GetTalentSkills()
    {
        List<int> skills = new();
        skills.AddRange(GetTalentGains(TalentGainsType.ActiveSkill));
        skills.AddRange(GetTalentGains(TalentGainsType.PassiveSkill));
        return skills;
    }

    /// <summary>
    /// 更新天赋节点
    /// </summary>
    /// <param name="newNode"></param>
    /// <param name="talentType"></param>
    public void UpdateNode(TalentNode newNode, TalentType talentType)
    {
        if (!AllTalentTreeDic.TryGetValue(talentType, out Dictionary<int, TalentNode> talentTreeDic))
        {
            Log.Error($"update talent node failed, can not find talent tree, talent type: {talentType}");
            return;
        }

        if (!talentTreeDic.ContainsKey(newNode.NodeId))
        {
            Log.Error($"update talent node failed, can not find node, node id: {newNode.NodeId}");
            return;
        }

        Log.Info($"update talent node, node id: {newNode.NodeId}");
        TalentNode oldNode = talentTreeDic[newNode.NodeId];
        // talentTreeDic[newNode.NodeId] = newNode;
        // List<GrpcTalentNodeData> talentList = _allTalentTreeList[talentType];
        //后面的节点更新的概率更大，所以从后往前遍历
        // for (int i = talentList.Count - 1; i >= 0; i--)
        // {
        //     if (talentList[i].NodeId == newNode.NodeId)
        //     {
        //         talentList[i] = newNode;
        //         break;
        //     }
        // }

        OnNodeUpdated(oldNode, newNode);
        oldNode.Level = newNode.Level;
    }

    /// <summary>
    /// 新增天赋节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="talentType"></param>
    public void AddTalentNode(TalentNode node, TalentType talentType)
    {
        if (!AllTalentTreeDic.TryGetValue(talentType, out Dictionary<int, TalentNode> talentTreeDic))
        {
            talentTreeDic = new();
            AllTalentTreeList.Add(talentType, new());
            AllTalentTreeDic.Add(talentType, talentTreeDic);
        }

        if (talentTreeDic.ContainsKey(node.NodeId))
        {
            Log.Error($"add talent node failed, node id: {node.NodeId} already exist");
            return;
        }

        Log.Info($"add talent node, node id: {node.NodeId}");
        talentTreeDic.Add(node.NodeId, node);
        AllTalentTreeList[talentType].Add(node);

        OnNodeAdded(node);
    }

    /// <summary>
    /// 移除天赋节点
    /// </summary>
    /// <param name="nodeID"></param>
    /// <param name="talentType"></param>
    public void RemoveTalentNode(int nodeID, TalentType talentType)
    {
        if (!AllTalentTreeDic.TryGetValue(talentType, out Dictionary<int, TalentNode> talentTreeDic))
        {
            Log.Error($"update talent node failed, can not find talent tree, talent type: {talentType}");
            return;
        }

        Log.Info($"remove talent node, node id: {nodeID}");
        if (!talentTreeDic.Remove(nodeID, out TalentNode removedNode))
        {
            Log.Error($"remove talent node failed, can not find node, node id: {nodeID}");
            return;
        }

        List<TalentNode> talentList = AllTalentTreeList[talentType];
        //删除节点从后往前找查询命中率会高一点，因为一般情况下，删除的节点都是最后一个
        for (int i = talentList.Count - 1; i >= 0; i--)
        {
            if (talentList[i].NodeId == nodeID)
            {
                talentList.RemoveAt(i);
                break;
            }
        }

        OnNodeRemoved(removedNode);
    }

    /// <summary>
    /// 当天赋节点更新
    /// </summary>
    /// <param name="oldNode"></param>
    /// <param name="newNode"></param>
    private void OnNodeUpdated(TalentNode oldNode, TalentNode newNode)
    {
        DRTalentTree nodeCfg = _refTalentTreeTable.GetDataRow(newNode.NodeId);
        if (nodeCfg == null)
        {
            Log.Error($"can not find talent tree config, node id: {newNode.NodeId}");
            return;
        }

        if (HasSkillTypeGains(nodeCfg))
        {
            List<int> addedSkillIDArr = TableUtil.GetTalentNodeSkillGains(nodeCfg, newNode.Level);
            List<int> removedSkillIDArr = TableUtil.GetTalentNodeSkillGains(nodeCfg, oldNode.Level);

            if (removedSkillIDArr != null || addedSkillIDArr != null)
            {
                //处理天赋技能数据变更
                RefEntity.EntityEvent.TalentSkillUpdated?.Invoke(addedSkillIDArr, removedSkillIDArr);
            }
        }
    }

    /// <summary>
    /// 当新增天赋节点
    /// </summary>
    /// <param name="node"></param>
    private void OnNodeAdded(TalentNode node)
    {
        DRTalentTree nodeCfg = _refTalentTreeTable.GetDataRow(node.NodeId);
        if (nodeCfg == null)
        {
            Log.Error($"can not find talent tree config, node id: {node.NodeId}");
            return;
        }

        if (HasSkillTypeGains(nodeCfg) && node.Level > 0)
        {
            //处理天赋技能数据变更
            List<int> addedSkillIDArr = TableUtil.GetTalentNodeSkillGains(nodeCfg, node.Level);
            RefEntity.EntityEvent.TalentSkillUpdated?.Invoke(addedSkillIDArr, null);
        }
    }

    /// <summary>
    /// 当天赋节点被移除时
    /// </summary>
    /// <param name="node"></param>
    private void OnNodeRemoved(TalentNode node)
    {
        DRTalentTree nodeCfg = _refTalentTreeTable.GetDataRow(node.NodeId);
        if (nodeCfg == null)
        {
            Log.Error($"can not find talent tree config, node id: {node.NodeId}");
            return;
        }

        if (HasSkillTypeGains(nodeCfg) && node.Level > 0)
        {
            //处理天赋技能数据变更
            List<int> removedSkillIDArr = TableUtil.GetTalentNodeSkillGains(nodeCfg, node.Level);
            RefEntity.EntityEvent.TalentSkillUpdated?.Invoke(null, removedSkillIDArr);
        }
    }

    /// <summary>
    /// 判断是否技能类型的收益
    /// </summary>
    /// <param name="gainsType"></param>
    /// <returns></returns>
    private bool HasSkillTypeGains(DRTalentTree cfg)
    {
        if (cfg == null)
        {
            return false;
        }

        return cfg.GainsType.Contains((int)TalentGainsType.ActiveSkill) || cfg.GainsType.Contains((int)TalentGainsType.PassiveSkill);
    }

    /// <summary>
    /// 查询玩家等级 (玩家等级 = 各类型天赋树中 masterLevel 的最大等级)
    /// </summary>
    /// <returns></returns>
    public int GetPlayerLevel()
    {
        int level = 0;
        foreach (TalentLevel treeLv in TalentTreeLvList)
        {
            if (level < treeLv.MasterLevel)
            {
                level = treeLv.MasterLevel;
            }
        }
        return level;
    }

    /// <summary>
    /// 获取玩家某类型天赋主干等级
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetPlayerTypeLevel(TalentType type)
    {
        foreach (TalentLevel treeLv in TalentTreeLvList)
        {
            if (treeLv.Type == type)
            {
                return treeLv.MasterLevel;
            }
        }

        //新用户，天赋树等级列表为空，正常返回0
        return 0;
    }
}