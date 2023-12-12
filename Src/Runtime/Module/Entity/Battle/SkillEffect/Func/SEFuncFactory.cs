/*
 * @Author: xiang huan
 * @Date: 2022-07-19 10:49:14
 * @Description: 技能效果球方法工厂
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Func/SEFuncFactory.cs
 * 
 */
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


public class SEFuncFactory
{
    private static SEFuncFactory s_instance;
    public static SEFuncFactory Inst
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new SEFuncFactory();
                s_instance.InitSEFuncMap();
            }
            return s_instance;
        }
    }

    protected Dictionary<eSEFuncType, Type> SEFuncMap;
    /// <summary>
    /// 初始化效果工厂Map
    /// </summary>
    public virtual void InitSEFuncMap()
    {
        SEFuncMap = new()
        {
            {eSEFuncType.SEFuncAddDamage, typeof(SEFuncAddDamage)},
            {eSEFuncType.SEFunApplySkillEffect, typeof(SEFunApplySkillEffect)},
            {eSEFuncType.SEFuncReset, typeof(SEFuncReset)},
        };
    }

    /// <summary>
    /// 创建技能效果事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="skillEffect">技能效果</param>
    /// <param name="funcData">事件数据</param>
    /// <returns></returns>
    public SEFuncBase CreateSEFunc(eSEFuncType type, SkillEffectBase skillEffect, object funcData = null)
    {
        if (SEFuncMap == null)
        {
            Log.Error($"CreateSEFunc Error not init skill effect map");
            return null;
        }
        if (!SEFuncMap.ContainsKey(type))
        {
            Log.Error($"CreateSEFunc Error type is Unknown  type = {type} ");
            return null;
        }
        Type funcClass = SEFuncMap[type];
        SEFuncBase seEvent = SEFuncBase.Create(funcClass);
        seEvent.SetData(type, skillEffect, funcData);
        seEvent.Init();
        return seEvent;
    }
}