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
public class DRSceneArea : DataRowBase
{
    private int _id = 0;

    /// <summary>
    /// /**获取id-int。*/
    /// </summary>
    public override int Id => _id;

    /// <summary>
  /**获取campLimit-int[]。*/
    /// </summary>
    public int[] CampLimit
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
  /**获取loadingBg-string。*/
    /// </summary>
    public string LoadingBg
    {
        get;
        private set;
    }

    /// <summary>
  /**获取rewardFrequency-int。*/
    /// </summary>
    public int RewardFrequency
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sceneName-string。*/
    /// </summary>
    public string SceneName
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sceneSubtype-int。*/
    /// </summary>
    public int SceneSubtype
    {
        get;
        private set;
    }

    /// <summary>
  /**获取sceneType-int。*/
    /// </summary>
    public int SceneType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取baseScore-int[]。*/
    /// </summary>
    public int[] BaseScore
    {
        get;
        private set;
    }

    /// <summary>
  /**获取bossCovers-int[]。*/
    /// </summary>
    public int[] BossCovers
    {
        get;
        private set;
    }

    /// <summary>
  /**获取challengeMode-int[]。*/
    /// </summary>
    public int[] ChallengeMode
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chapterBaseReward-int[]。*/
    /// </summary>
    public int[] ChapterBaseReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chapterLuckyReward-int[]。*/
    /// </summary>
    public int[] ChapterLuckyReward
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chapterPunish-int[][]。*/
    /// </summary>
    public int[][] ChapterPunish
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chapterTimeLimit-int[]。*/
    /// </summary>
    public int[] ChapterTimeLimit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取chapters-string[]。*/
    /// </summary>
    public string[] Chapters
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
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取punishDesc-string。*/
    /// </summary>
    public string PunishDesc
    {
        get;
        private set;
    }

    /// <summary>
  /**获取tickets-int[][]。*/
    /// </summary>
    public int[][] Tickets
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        CampLimit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        Level = DataTableParseUtil.ParseInt(columnStrings[index++]);
        LoadingBg = DataTableParseUtil.ParseString(columnStrings[index++]);
        RewardFrequency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SceneName = DataTableParseUtil.ParseString(columnStrings[index++]);
        SceneSubtype = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SceneType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        BaseScore = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        BossCovers = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChallengeMode = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterBaseReward = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterLuckyReward = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterPunish = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        ChapterTimeLimit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Chapters = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        PunishDesc = DataTableParseUtil.ParseString(columnStrings[index++]);
        Tickets = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                CampLimit = binaryReader.ReadArray<Int32>();
                _id = binaryReader.Read7BitEncodedInt32();
                Level = binaryReader.Read7BitEncodedInt32();
                LoadingBg = binaryReader.ReadString();
                RewardFrequency = binaryReader.Read7BitEncodedInt32();
                SceneName = binaryReader.ReadString();
                SceneSubtype = binaryReader.Read7BitEncodedInt32();
                SceneType = binaryReader.Read7BitEncodedInt32();
                BaseScore = binaryReader.ReadArray<Int32>();
                BossCovers = binaryReader.ReadArray<Int32>();
                ChallengeMode = binaryReader.ReadArray<Int32>();
                ChapterBaseReward = binaryReader.ReadArray<Int32>();
                ChapterLuckyReward = binaryReader.ReadArray<Int32>();
                ChapterPunish = binaryReader.ReadArrayList<Int32>();
                ChapterTimeLimit = binaryReader.ReadArray<Int32>();
                Chapters = binaryReader.ReadArray<String>();
                Desc = binaryReader.ReadString();
                Name = binaryReader.ReadString();
                PunishDesc = binaryReader.ReadString();
                Tickets = binaryReader.ReadArrayList<Int32>();
            }
        }

        return true;
    }
}

