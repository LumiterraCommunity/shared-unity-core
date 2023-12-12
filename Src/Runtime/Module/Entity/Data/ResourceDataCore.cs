/*
 * @Author: xiang huan
 * @Date: 2022-10-14 15:42:12
 * @Description: 家园资源数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Data/ResourceDataCore.cs
 * 
 */

using UnityGameFramework.Runtime;

/// <summary>
/// 家园资源数据
/// </summary>
public class ResourceDataCore : EntityBaseComponent
{
    public int ConfigId => DRHomeResources == null ? -1 : DRHomeResources.Id;
    public DRHomeResources DRHomeResources { get; protected set; }

    public HomeResourcesPointSaveData SaveData { get; protected set; }

    public void SetConfigID(int cfgID)
    {
        DRHomeResources = GFEntryCore.DataTable.GetDataTable<DRHomeResources>().GetDataRow(cfgID);

        if (DRHomeResources == null)
        {
            Log.Error($"Can not find DRHomeResources cfg id:{cfgID}");
        }
        else
        {
            EntityAttributeData attributeData = GetComponent<EntityAttributeData>();
            attributeData.SetBaseValue(eAttributeType.GrassDef, DRHomeResources.GrassDef);
            attributeData.SetBaseValue(eAttributeType.TreeDef, DRHomeResources.TreeDef);
            attributeData.SetBaseValue(eAttributeType.OreDef, DRHomeResources.OreDef);
        }
    }

    public void SetSaveData(HomeResourcesPointSaveData saveData)
    {
        SaveData = saveData;
    }
}