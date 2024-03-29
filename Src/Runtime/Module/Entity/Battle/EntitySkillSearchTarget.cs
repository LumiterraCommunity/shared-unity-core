/*
 * @Author: xiang huan
 * @Date: 2022-07-29 10:08:50
 * @Description: 实体技能搜索目标
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/EntitySkillSearchTarget.cs
 * 
 */


using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EntitySkillSearchTarget : EntityBaseComponent
{
    public List<EntityBase> TargetList { get; protected set; } = new();//目标实体
    public List<long> TargetIDList { get; protected set; } = new();//目标ID
    public List<Vector3> TargetPosList { get; protected set; } = new(); //目标位置
    public Vector3 TargetDir { get; protected set; } = Vector3.forward; //目标方向
    public int TargetNum { get; protected set; } = 1;//需要的目标数量
    public int SearchNum { get; protected set; } = 0;//已经搜索到的目标数量
    protected EntityInputData InputData;
    protected virtual void Start()
    {
        InputData = GetComponent<EntityInputData>();
    }
    private void OnDestroy()
    {
        ResetSearch();
    }

    public virtual void ResetSearch()
    {
        TargetList.Clear();
        TargetIDList.Clear();
        TargetPosList.Clear();
        TargetDir = Vector3.forward;
        SearchNum = 0;
        TargetNum = 0;
    }
    /// <summary>
    /// 添加目标
    /// </summary>
    /// <param name="targetEntities">目标实体</param> 
    public virtual void AddTarget(List<EntityBase> targetEntities)
    {
        if (targetEntities == null || targetEntities.Count <= 0)
        {
            return;
        }
        TargetList.AddRange(targetEntities);
        TargetIDList.AddRange(targetEntities.ConvertAll(t => t.BaseData.Id));
    }

    /// <summary>
    /// 添加位置
    /// </summary>
    /// <param name="posList"></param> <summary>
    public virtual void AddTargetPos(List<Vector3> posList)
    {
        if (posList == null || posList.Count <= 0)
        {
            return;
        }
        TargetPosList.AddRange(posList);
    }

    /// <summary>
    /// 更新方向
    /// </summary>
    public virtual void UpdateTargetDir(Vector3 dir)
    {
        TargetDir = dir;
    }
    /// <summary>
    /// 搜索目标
    /// <summary>
    /// <param name="dir">搜素方向</param>/// 
    /// <param name="skillID">技能ID</param>
    /// <param name="targetNum">目标数量</param> 
    public virtual void SearchTarget(Vector3 dir, int skillID, int targetNum = 1)
    {
        ResetSearch();
        UpdateTargetDir(dir);
        TargetNum = targetNum;

        DRSkill drSkill = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(skillID);
        if (drSkill == null)
        {
            Log.Error($"not find skill id:{skillID}");
            return;
        }

        for (int i = 0; i < drSkill.SearchTarget.Length; i++)
        {
            SearchNum += SkillSearchHelp.SearchTarget(this, dir, drSkill, drSkill.SearchTarget[i]);
            if (SearchNum >= TargetNum)
            {
                break;
            }
        }

    }

}
