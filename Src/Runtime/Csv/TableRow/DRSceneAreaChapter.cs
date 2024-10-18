//------------------------------------------------------------
// 此文件由工具自动生成
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;


/// <summary>
/** __DATA_TABLE_COMMENT__*/
/// </summary>
public class DRSceneAreaChapter : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取bossEntityList-int[]。*/
    /// </summary>
    public int[] BossEntityList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取entityList-int[][]。*/
    /// </summary>
    public int[][] EntityList
    {
        get;
        private set;
    }

    /// <summary>
  /**获取entityMaximum-int。*/
    /// </summary>
    public int EntityMaximum
    {
        get;
        private set;
    }

    /// <summary>
  /**获取entityType-int。*/
    /// </summary>
    public int EntityType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sceneAreaId-int。*/
    /// </summary>
    public int SceneAreaId
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        BossEntityList = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        EntityList = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        EntityMaximum = DataTableParseUtil.ParseInt(columnStrings[index++]);
        EntityType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        SceneAreaId = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                BossEntityList = binaryReader.ReadArray<Int32>();
                EntityList = binaryReader.ReadArrayList<Int32>();
                EntityMaximum = binaryReader.Read7BitEncodedInt32();
                EntityType = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                Name = binaryReader.ReadString();
                SceneAreaId = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

