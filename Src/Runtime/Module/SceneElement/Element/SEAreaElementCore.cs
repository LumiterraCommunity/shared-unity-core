﻿/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 技能效果区域
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/SEAreaElementCore.cs
 * 
 */
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class SEAreaElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.SEArea;
    [Header("触发类型")]
    public eSEAreaTriggerType TriggerType;

    [Header("技能ID")]
    public int SkillID = 14;

    [Header("技能效果ID列表")]
    public int[] EffectIDs;

    [Header("触发间隔时间(s)")]
    public float Interval = 1f;


    protected float LastTriggerTime = 0;
    protected EntityTriggerHelper TriggerHelper;
    protected InputSkillReleaseData InputData;

    protected virtual void Start()
    {
        TriggerHelper = gameObject.AddComponent<EntityTriggerHelper>();
        TriggerHelper.OnAddEntity += OnAddEntity;
        TriggerHelper.OnRemoveEntity += OnRemoveEntity;

        InputData = new(SkillID, transform.forward, transform.position);
        InputData.SetInputRandomSeed(0); // 固定随机值
    }
    protected override void Update()
    {
        base.Update();
        if (TriggerType == eSEAreaTriggerType.Interval)
        {
            LastTriggerTime -= Time.deltaTime;
            if (LastTriggerTime <= 0)
            {
                LastTriggerTime = Interval;
                OnTriggerInterval();
            }
        }

    }
    protected virtual void OnTriggerInterval()
    {
        if (TriggerHelper.EntityDic.Count == 0)
        {
            return;
        }

        for (int i = 0; i < TriggerHelper.EntityDic.Count; i++)
        {
            EntityBase entity = TriggerHelper.EntityDic[i];
            EntitySkillEffectExecute(entity);
        }
    }

    protected virtual void OnAddEntity(Collider other, EntityBase entity)
    {
        if (TriggerType == eSEAreaTriggerType.Enter)
        {
            EntitySkillEffectExecute(entity);
        }

    }

    protected virtual void OnRemoveEntity(Collider other, EntityBase entity)
    {
        if (TriggerType == eSEAreaTriggerType.Exit)
        {
            EntitySkillEffectExecute(entity);
        }
    }

    protected void EntitySkillEffectExecute(EntityBase entity)
    {
        if (!entity.Inited || !entity.BattleDataCore.IsLive())
        {
            return;
        }
        _ = SkillUtil.EntitySkillEffectExecute(InputData, EffectIDs, entity, entity);
    }
}
