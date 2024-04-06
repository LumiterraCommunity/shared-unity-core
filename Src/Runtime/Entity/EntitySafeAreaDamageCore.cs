/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体安全区伤害组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntitySafeAreaDamageCore.cs
 * 
 */


using System.Collections.Generic;
using System.Runtime.InteropServices;


/// <summary>
/// 实体安全区伤害组件
/// </summary>
public class EntitySafeAreaDamageCore : EntityBaseComponent
{
    public bool IsSafeArea { get; private set; } = true;
    public List<SceneElementCore> SafeAreaElements = new();
    private void Start()
    {
        StartCheckSafeArea();
    }

    private void OnDestroy()
    {
        StopCheckSafeArea();
    }

    public void StartCheckSafeArea()
    {
        StopCheckSafeArea();
        TimerMgr.AddTimer(GetHashCode(), TimeUtil.S2MS, CheckSafeArea, 0);
    }

    public void StopCheckSafeArea()
    {
        _ = TimerMgr.RemoveTimer(GetHashCode());
    }
    private void CheckSafeArea()
    {
        IsSafeArea = true;
        //死亡不受伤害
        if (!RefEntity.BattleDataCore.IsLive())
        {
            return;
        }

        SafeAreaElementCore safeAreaElement = GetCurSafeAreaElement();
        if (safeAreaElement == null)
        {
            return;
        }
        if (!safeAreaElement.IsSafeArea(RefEntity.Position))
        {
            _ = AreaDamage(safeAreaElement.GetCurSafeAreaInfo());
            IsSafeArea = false;
        }
    }


    protected SafeAreaElementCore GetCurSafeAreaElement()
    {
        if (!GFEntryCore.IsExistModule<SceneElementMgrCore>())
        {
            return null;
        }

        if (!RefEntity.TryGetComponent(out EntityBattleArea entityBattleArea))
        {
            return null;
        }
        SceneElementMgrCore sceneElementMgr = GFEntryCore.GetModule<SceneElementMgrCore>();
        SafeAreaElements.Clear();
        sceneElementMgr.GetSceneElementListByTypeAndAreaID(eSceneElementType.SafeArea, entityBattleArea.CurAreaID, SafeAreaElements);
        if (SafeAreaElements == null || SafeAreaElements.Count <= 0)
        {
            return null;
        }
        //理论上一个战斗区域只有一个安全区，否则设计有问题
        return SafeAreaElements[0] as SafeAreaElementCore;
    }
    protected virtual int AreaDamage(SafeAreaElementCore.SafeAreaInfo safeAreaInfo)
    {
        int damage = (int)(safeAreaInfo.Damage * RefEntity.BattleDataCore.HPMAX / 100);
        if (RefEntity.BattleDataCore.IsLive())
        {
            RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(BattleDefine.SCENE_DAMAGE_ENTITY_ID, damage);
        }

        int hp = RefEntity.BattleDataCore.HP - damage;
        RefEntity.BattleDataCore.SetHP(hp);
        return damage;
    }
}