using System;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// 整个家园的存储数据 可能是数据库来的 也可能是给客户端协议里解来的
/// </summary>
[Serializable]
public class HomeSaveData
{
    /// <summary>
    /// 所有非空白idle状态的土地列表 空白的不存储
    /// </summary>
    public List<SoilSaveData> SoilSaveDataList;

    public List<HomeResourcesAreaSaveData> HomeAreaSaveDataList;
    public List<AnimalSaveData> AnimalSaveDataList;
    public AnimalSceneSaveData AnimalSceneSaveData;
    /// <summary>
    /// 上次保存的时间戳
    /// </summary>
    public long LastSaveStamp;

    // 是否使用初始家园数据
    public bool UseDefaultData;

    private static readonly eCompressType s_compressType = eCompressType.Deflate;

    /// <summary>
    /// 转Json
    /// </summary>
    /// <param name="data"></param>
    /// <param name="compression">是否需要压缩 默认false</param>
    /// <returns></returns>
    public static string ToJson(object data, bool compression = false)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.None);
        if (!compression)
        {
            return json;
        }

        byte[] bs = CompressionUtil.CompressString(json, s_compressType);
        return Convert.ToBase64String(bs);//为什么用base64 因为utf8编码后的byte[]转string会丢失数据
    }

    /// <summary>
    /// 从json转回来结构
    /// </summary>
    /// <param name="json"></param>
    /// <param name="compression">是否压缩过 默认false</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FromJson<T>(string json, bool compression = false)
    {
        if (!compression)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        byte[] compressedBs = Convert.FromBase64String(json);//用base64转回来原因同ToJson
        string originalJson = CompressionUtil.DecompressString(compressedBs, s_compressType);
        return JsonConvert.DeserializeObject<T>(originalJson);
    }
}