/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体安全区伤害组件
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntitySafeAreaDamageCore.cs
 * 
 */


using System.Collections.Generic;


/// <summary>
/// 实体安全区伤害组件
/// </summary>
public class EntitySafeAreaDamageCore : EntityBaseComponent
{

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
        TimerMgr.AddTimer(GetHashCode(), TimeUtil.S2MS, CheckSafeArea, 0);
    }

    public void StopCheckSafeArea()
    {
        _ = TimerMgr.RemoveTimer(GetHashCode());
    }
    private void CheckSafeArea()
    {
        if (!GFEntryCore.IsExistModule<SceneElementMgrCore>())
        {
            return;
        }

        SceneElementMgrCore sceneElementMgr = GFEntryCore.GetModule<SceneElementMgrCore>();
        List<SafeAreaElementCore> safeAreaElements = sceneElementMgr.GetSceneElementListByType<SafeAreaElementCore>(eSceneElementType.SafeArea);
        if (safeAreaElements == null || safeAreaElements.Count <= 0)
        {
            return;
        }
        if (!RefEntity.BattleDataCore.IsLive())
        {
            return;
        }

        //和平区域不会受到伤害
        if (RefEntity.TryGetComponent(out EntityBattleArea entityBattleArea) && entityBattleArea.CurAreaType == eBattleAreaType.Peace)
        {
            return;
        }

        //理论上只有一个安全区，否则设计有问题
        SafeAreaElementCore safeAreaElement = safeAreaElements[0];
        if (!safeAreaElement.IsSafeArea(RefEntity.Position))
        {
            AreaDamage(safeAreaElement.GetCurSafeAreaInfo());
        }
    }

    protected virtual void AreaDamage(SafeAreaElementCore.SafeAreaInfo safeAreaInfo)
    {
        int damage = (int)(safeAreaInfo.Damage * RefEntity.BattleDataCore.HPMAX / 100);
        if (RefEntity.BattleDataCore.IsLive())
        {
            RefEntity.EntityEvent.EntityBattleAddDamage?.Invoke(BattleDefine.SCENE_DAMAGE_ENTITY_ID, damage);
        }

        int hp = RefEntity.BattleDataCore.HP - damage;
        RefEntity.BattleDataCore.SetHP(hp);
    }
}