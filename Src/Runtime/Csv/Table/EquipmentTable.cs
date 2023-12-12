using System.Collections.Generic;
using GameFramework.DataTable;

public class EquipmentTable
{
    private readonly DREquipment[] _rows;
    private static EquipmentTable s_inst;
    public static EquipmentTable Inst
    {
        get
        {
            if (s_inst == null)
            {
                s_inst = new EquipmentTable();
            }
            return s_inst;
        }
    }

    private Dictionary<int, List<DREquipment>> _dic { get; set; } = new();
    public EquipmentTable()
    {
        IDataTable<DREquipment> dt = GFEntryCore.DataTable.GetDataTable<DREquipment>();
        _rows = dt.GetAllDataRows();
        foreach (DREquipment row in _rows)
        {
            if (!_dic.TryGetValue(row.ItemId, out List<DREquipment> qualityList))
            {
                qualityList = new();
                _dic.Add(row.ItemId, qualityList);
            }

            //按品质从低到高排序
            int index = 0;
            for (; index < qualityList.Count; index++)
            {
                if (qualityList[index].GearQuality > row.GearQuality)
                {
                    break;
                }
            }

            qualityList.Insert(index, row);
        }
    }

    /// <summary>
    /// 通过装备的itemID获取装备的配置数据
    /// 默认返回品质最低的
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public DREquipment GetRowByItemID(int itemID)
    {
        if (_dic.ContainsKey(itemID))
        {
            return _dic[itemID]?[0];
        }

        return null;
    }

    /// <summary>
    /// 通过装备的itemID和品质获取装备的配置数据
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public DREquipment GetRowByItemID(int itemID, int quality)
    {
        if (_dic.TryGetValue(itemID, out List<DREquipment> qualityList))
        {
            for (int i = 0; i < qualityList.Count; i++)
            {
                DREquipment row = qualityList[i];
                if (row.GearQuality == quality)
                {
                    return row;
                }
            }
        }

        return null;
    }
}