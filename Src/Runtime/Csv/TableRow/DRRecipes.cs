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
public class DRRecipes : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取classifyLevel-int。*/
    /// </summary>
    public int ClassifyLevel
    {
        get;
        private set;
    }

    /// <summary>
  /**获取desc-string。*/
    /// </summary>
    public string Desc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取displayType-int。*/
    /// </summary>
    public int DisplayType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取icon-string。*/
    /// </summary>
    public string Icon
    {
        get;
        private set;
    }

    /// <summary>
  /**获取level-int。*/
    /// </summary>
    public int Level
    {
        get;
        private set;
    }

    /// <summary>
  /**获取matItemId-int[][]。*/
    /// </summary>
    public int[][] MatItemId
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
  /**获取productId-int[][]。*/
    /// </summary>
    public int[][] ProductId
    {
        get;
        private set;
    }

    /// <summary>
  /**获取productTypes-int[]。*/
    /// </summary>
    public int[] ProductTypes
    {
        get;
        private set;
    }

    /// <summary>
  /**获取proficiency-int。*/
    /// </summary>
    public int Proficiency
    {
        get;
        private set;
    }

    /// <summary>
  /**获取recipesSort-int。*/
    /// </summary>
    public int RecipesSort
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sourceText-int。*/
    /// </summary>
    public int SourceText
    {
        get;
        private set;
    }

    /// <summary>
  /**获取timeCost-int。*/
    /// </summary>
    public int TimeCost
    {
        get;
        private set;
    }

    /// <summary>
  /**获取timesLimit-int。*/
    /// </summary>
    public int TimesLimit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取type-int。*/
    /// </summary>
    public int Type
    {
        get;
        private set;
    }

    /// <summary>
  /**获取unlockCondition-int。*/
    /// </summary>
    public int UnlockCondition
    {
        get;
        private set;
    }

    /// <summary>
  /**获取unlockType-int。*/
    /// </summary>
    public int UnlockType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取useDitamin-int。*/
    /// </summary>
    public int UseDitamin
    {
        get;
        private set;
    }

    /// <summary>
  /**获取useToken-int。*/
    /// </summary>
    public int UseToken
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        ClassifyLevel = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        DisplayType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Icon = DataTableParseUtil.ParseString(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Level = DataTableParseUtil.ParseInt(columnStrings[index++]);
        MatItemId = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        ProductId = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        ProductTypes = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Proficiency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RecipesSort = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SourceText = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TimeCost = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TimesLimit = DataTableParseUtil.ParseInt(columnStrings[index++]);
        Type = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UnlockCondition = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UnlockType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UseDitamin = DataTableParseUtil.ParseInt(columnStrings[index++]);
        UseToken = DataTableParseUtil.ParseInt(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                ClassifyLevel = binaryReader.Read7BitEncodedInt32();
                Desc = binaryReader.ReadString();
                DisplayType = binaryReader.Read7BitEncodedInt32();
                Icon = binaryReader.ReadString();
                _id = binaryReader.Read7BitEncodedInt32();
                Level = binaryReader.Read7BitEncodedInt32();
                MatItemId = binaryReader.ReadArrayList<Int32>();
                Name = binaryReader.ReadString();
                ProductId = binaryReader.ReadArrayList<Int32>();
                ProductTypes = binaryReader.ReadArray<Int32>();
                Proficiency = binaryReader.Read7BitEncodedInt32();
                RecipesSort = binaryReader.Read7BitEncodedInt32();
                SourceText = binaryReader.Read7BitEncodedInt32();
                TimeCost = binaryReader.Read7BitEncodedInt32();
                TimesLimit = binaryReader.Read7BitEncodedInt32();
                Type = binaryReader.Read7BitEncodedInt32();
                UnlockCondition = binaryReader.Read7BitEncodedInt32();
                UnlockType = binaryReader.Read7BitEncodedInt32();
                UseDitamin = binaryReader.Read7BitEncodedInt32();
                UseToken = binaryReader.Read7BitEncodedInt32();
            }
        }

        return true;
    }
}

