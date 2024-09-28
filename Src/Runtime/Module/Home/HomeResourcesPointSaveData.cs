using System;

/// <summary>
/// 单个资源 可能是数据库的  也可能是服务器转给客户端的
/// </summary>
[Serializable]
public class HomeResourcesPointSaveData
{
    /// <summary>
    /// 唯一ID 在家园里时表示所在土地ID
    /// </summary>
    public ulong Id;
    /// <summary>
    /// 配置ID
    /// </summary>
    public int Cid;
    /// <summary>
    /// 位置
    /// </summary>
    public SerializableVector3 Pos;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level;
}