/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 层级数量触发效果,>=最大层级时触发，重置层级为0
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SELayerTriggerCore.cs
* 
*/

using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class SELayerTriggerCore : SkillEffectBase
{
    public override bool IsUpdate => true;
    public override bool IsStaticSync => true;
    protected List<SEFuncBase> FuncList;
    protected List<SEFuncBase> UpdateFuncList;
    protected int[] EffectIDList;
    public override void OnAdd()
    {
        base.OnAdd();
        FuncList = new();
        UpdateFuncList = new();
        EffectIDList = EffectCfg.Parameters;
    }

    public override void OnRemove()
    {
        if (FuncList != null)
        {
            for (int i = 0; i < FuncList.Count; i++)
            {
                FuncList[i].Dispose();
            }
            FuncList = null;
        }
        UpdateFuncList = null;
        EffectIDList = null;
        base.OnRemove();
    }

    public override void Update()
    {
        if (UpdateFuncList.Count > 0)
        {
            for (int i = 0; i < UpdateFuncList.Count; i++)
            {
                UpdateFuncList[i].Update();
            }
        }
    }
    public override void UpdateLayer(int layer)
    {
        //冷却中
        long curTimeStamp = TimeUtil.GetServerTimeStamp();
        if (curTimeStamp < NextIntervalTime)
        {
            return;
        }

        //层级更新
        base.UpdateLayer(layer);

        //层级触发
        if (CurLayer >= EffectCfg.MaxLayer)
        {
            UpdateIntervalTime(curTimeStamp + EffectCfg.EffectInterval);
            UpdateTrigger();
            base.UpdateLayer(1);
        }
    }
    protected virtual void UpdateTrigger()
    {

    }
    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }
        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }

        SEFuncBase funcReset = SEFuncFactory.Inst.CreateSEFunc(eSEFuncType.SEFuncReset, this, null);
        FuncList.Add(funcReset);

        //方法列表
        for (int i = 0; i < EffectCfg.Parameters2.Length; i++)
        {
            if (EffectCfg.Parameters2[i].Length < 1)
            {
                Log.Error($"SELayerTriggerCore Parameters2 Error EffectID = {EffectID}");
                continue;
            }

            eSEFuncType funcType = (eSEFuncType)EffectCfg.Parameters2[i][0];
            SEFuncBase funcBase = SEFuncFactory.Inst.CreateSEFunc(funcType, this, EffectCfg.Parameters2[i]);
            if (funcBase != null)
            {
                FuncList.Add(funcBase);
                if (funcBase.IsUpdate)
                {
                    UpdateFuncList.Add(funcBase);
                }
            }
        }

    }
}