/*
 * @Author: xiang huan
 * @Date: 2022-10-14 15:42:12
 * @Description: npc数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/NpcDataCore.cs
 * 
 */

using UnityGameFramework.Runtime;

/// <summary>
/// 怪物基础数据
/// </summary>
public class NpcDataCore : EntityBaseComponent
{
    public int ConfigId => DRNpc == null ? -1 : DRNpc.Id;
    public DRNpc DRNpc { get; protected set; }


    public void SetConfigID(int cfgID)
    {
        DRNpc = GFEntryCore.DataTable.GetDataTable<DRNpc>().GetDataRow(cfgID);

        if (DRNpc == null)
        {
            Log.Error($"Can not find npc cfg id:{cfgID}");
        }
    }
}