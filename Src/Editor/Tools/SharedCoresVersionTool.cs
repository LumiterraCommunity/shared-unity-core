/*
 * @Author: xiang huan
 * @Date: 2022-07-18 16:03:05
 * @Description: 共享库资源版本管理
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Editor/Tools/SharedCoresVersionTool.cs
 * 
 */
using System.IO;
using UnityEditor;
using UnityEngine;
public class SharedCoresVersion
{
    public int? ConfigVersion; //服务器场景数据配置版本 例如，刷新点，攻占区域等
    public int? CsvVersion;//表版本
    public int? SceneVersion;//场景版本
    public int? NavMeshVersion;//寻路数据版本
}

public static class SharedCoresVersionTool
{
    public static string CLIENT_SHARED_CORE_VERSION = "Assets/Res/SharedCoreVersion.json";
    public static string SHARED_CORE_VERSION = "Assets/Plugins/SharedCore/Res/SharedCoreVersion.json";

    public static SharedCoresVersion ReadSharedCoresVersion()
    {
        if (File.Exists(CLIENT_SHARED_CORE_VERSION))
        {
            string versionJson = File.ReadAllText(CLIENT_SHARED_CORE_VERSION);
            return Unity.Plastic.Newtonsoft.Json.JsonConvert.DeserializeObject<SharedCoresVersion>(versionJson);
        }
        else
        {
            return new SharedCoresVersion();
        }
    }

    public static void WriteSharedCoresVersion(SharedCoresVersion versionData)
    {
        try
        {
            string dataJson = Unity.Plastic.Newtonsoft.Json.JsonConvert.SerializeObject(versionData);
            File.WriteAllText(CLIENT_SHARED_CORE_VERSION, dataJson);
            File.WriteAllText(SHARED_CORE_VERSION, dataJson);
        }
        catch (System.Exception)
        {
            Debug.LogError("Write SharedCoresVersion Error");
        }
    }
    public static void UpdateConfigVersion()
    {
        SharedCoresVersion versionData = ReadSharedCoresVersion();
        versionData.ConfigVersion = versionData.ConfigVersion != null ? versionData.ConfigVersion + 1 : 0;
        WriteSharedCoresVersion(versionData);
    }

    public static void UpdateCsvVersion()
    {
        SharedCoresVersion versionData = ReadSharedCoresVersion();
        versionData.CsvVersion = versionData.CsvVersion != null ? versionData.CsvVersion + 1 : 0;
        WriteSharedCoresVersion(versionData);
    }
    public static void UpdateSceneVersion()
    {
        SharedCoresVersion versionData = ReadSharedCoresVersion();
        versionData.SceneVersion = versionData.SceneVersion != null ? versionData.SceneVersion + 1 : 0;
        WriteSharedCoresVersion(versionData);
    }
    public static void UpdateNavMeshVersion()
    {
        SharedCoresVersion versionData = ReadSharedCoresVersion();
        versionData.NavMeshVersion = versionData.NavMeshVersion != null ? versionData.NavMeshVersion + 1 : 0;
        WriteSharedCoresVersion(versionData);
    }

}