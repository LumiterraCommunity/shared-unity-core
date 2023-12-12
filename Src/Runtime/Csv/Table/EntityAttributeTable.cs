using System.Collections.Generic;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体属性表
/// </summary>
public class EntityAttributeTable
{
    //伤害分类
    private Dictionary<HomeDefine.eAction, TableDamageAttributeCollections> _damageAttributeClassifyMap;

    //收获属性分类
    private Dictionary<HomeDefine.eAction, TableHarvestAttributeCollections> _harvestAttributeClassifyMap;

    private static EntityAttributeTable s_instance;

    public static EntityAttributeTable Inst
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new EntityAttributeTable();
            }

            return s_instance;
        }
    }

    private EntityAttributeTable()
    {
        InitDamageAttributeClassify();

        InitHarvestAttributeClassify();
    }

    /// <summary>
    /// 获取伤害属性分类
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public T GetDamageAttributeClassify<T>(HomeDefine.eAction action) where T : TableDamageAttributeCollections
    {
        if (!_damageAttributeClassifyMap.TryGetValue(action, out TableDamageAttributeCollections attribute))
        {
            Log.Error($"EntityAttributeTable GetDamageAttributeClassify Not Find Action = {action}");
            return null;
        }

        T t = attribute as T;
        if (t == null)
        {
            Log.Error($"EntityAttributeTable GetDamageAttributeClassify type error Action = {action} Type = {typeof(T)}");
        }

        return t;
    }

    /// <summary>
    /// 获取收获属性分类
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public TableHarvestAttributeCollections GetHarvestAttributeClassify(HomeDefine.eAction action)
    {
        if (!_harvestAttributeClassifyMap.TryGetValue(action, out TableHarvestAttributeCollections attribute))
        {
            Log.Error($"EntityAttributeTable GetHarvestAttributeClassify Not Find Action = {action}");
            return null;
        }

        return attribute;
    }

    private void InitHarvestAttributeClassify()
    {
        _harvestAttributeClassifyMap = new();

        //砍树
        TableHarvestAttributeCollections cutTree = new()
        {
            ExtraHarvestRate = eAttributeType.ExtraWoodRate,
        };
        _harvestAttributeClassifyMap.Add(HomeDefine.eAction.Cut, cutTree);

        //采矿
        TableHarvestAttributeCollections mining = new()
        {
            ExtraHarvestRate = eAttributeType.ExtraOreRate,
        };
        _harvestAttributeClassifyMap.Add(HomeDefine.eAction.Mining, mining);

        //农作物
        TableHarvestAttributeCollections harvest = new()
        {
            ExtraHarvestRate = eAttributeType.ExtraHarvestRate,
        };
        _harvestAttributeClassifyMap.Add(HomeDefine.eAction.Harvest, harvest);
    }

    private void InitDamageAttributeClassify()
    {
        _damageAttributeClassifyMap = new();

        //怪物 敌人
        TableEnemyDamageAttribute attack = new()
        {
            Att = eAttributeType.CombatAtt,
            DmgBonus = eAttributeType.CombatDmgBonus,
            CritRate = eAttributeType.CombatCritRate,
            CritDmg = eAttributeType.CombatCritDmg,
            Def = eAttributeType.CombatDef,
            Vulnerable = eAttributeType.CombatVulnerable,
        };
        _damageAttributeClassifyMap.Add(HomeDefine.eAction.AttackEnemy, attack);

        //割草
        TableHomeDamageAttribute mowingGrass = new()
        {
            Att = eAttributeType.GrassAtt,
            Def = eAttributeType.GrassDef,
            DmgBonus = eAttributeType.GrassDmgBonus,
            CritRate = eAttributeType.GrassCritRate,
            CritDmg = eAttributeType.GrassCritDmg,
        };
        _damageAttributeClassifyMap.Add(HomeDefine.eAction.Mowing, mowingGrass);

        //砍树
        TableHomeDamageAttribute cutTree = new()
        {
            Att = eAttributeType.TreeAtt,
            Def = eAttributeType.TreeDef,
            DmgBonus = eAttributeType.TreeDmgBonus,
            CritRate = eAttributeType.TreeCritRate,
            CritDmg = eAttributeType.TreeCritDmg,
        };
        _damageAttributeClassifyMap.Add(HomeDefine.eAction.Cut, cutTree);

        //采矿
        TableHomeDamageAttribute mining = new()
        {
            Att = eAttributeType.OreAtt,
            Def = eAttributeType.OreDef,
            DmgBonus = eAttributeType.OreDmgBonus,
            CritRate = eAttributeType.OreCritRate,
            CritDmg = eAttributeType.OreCritDmg,
        };

        _damageAttributeClassifyMap.Add(HomeDefine.eAction.Mining, mining);
    }
}