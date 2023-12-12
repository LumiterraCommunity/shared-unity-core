/** 
 * @Author XQ
 * @Date 2022-08-09 09:51:55
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/MonsterDataCore.cs
 */
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 怪物基础数据
/// </summary>
public class MonsterDataCore : EntityBaseComponent
{
    public int configId => DRMonster == null ? -1 : DRMonster.Id;
    public DRMonster DRMonster { get; protected set; }

    public void SetMonsterConfigID(int cfgID)
    {
        DRMonster = GFEntryCore.DataTable.GetDataTable<DRMonster>().GetDataRow(cfgID);

        if (DRMonster == null)
        {
            Log.Error($"Can not find monster cfg id:{cfgID}");
        }
    }

    // 拥有指定捕获技能
    public bool HasCaptureSkillId(int skillId)
    {

        for (int i = 0; i < DRMonster.CaptureSkillCastPool.Length; i++)
        {
            if (DRMonster.CaptureSkillCastPool[i][0] == skillId)
            {
                return true;
            }
        }


        return false;
    }

    // 是否是怪物喜欢的item
    public bool IsFavoriteItem(int itemCid)
    {
        for (int i = 0; i < DRMonster.FavoriteItem.Length; i++)
        {
            if (DRMonster.FavoriteItem[i] == itemCid)
            {
                return true;
            }
        }

        return false;
    }
}