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
  /**获取canInvest-bool。*/
    /// </summary>
    public bool CanInvest
    {
        get;
        private set;
    }

    /// <summary>
  /**获取extraDropPool-int。*/
    /// </summary>
    public int ExtraDropPool
    {
        get;
        private set;
    }

    /// <summary>
  /**获取isRanking-bool。*/
    /// </summary>
    public bool IsRanking
    {
        get;
        private set;
    }

    /// <summary>
  /**获取level-int[]。*/
    /// </summary>
    public int[] Level
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
  /**获取name-string。*/
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
  /**获取refreshRatio-int[]。*/
    /// </summary>
    public int[] RefreshRatio
    {
        get;
        private set;
    }

    /// <summary>
  /**获取reputationRequaireScore-int。*/
    /// </summary>
    public int ReputationRequaireScore
    {
        get;
        private set;
    }

    /// <summary>
  /**获取respawnTimes-int。*/
    /// </summary>
    public int RespawnTimes
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
  /**获取talentType-int。*/
    /// </summary>
    public int TalentType
    {
        get;
        private set;
    }

    /// <summary>
  /**获取worldDropProbability-int。*/
    /// </summary>
    public int WorldDropProbability
    {
        get;
        private set;
    }

    /// <summary>
  /**获取functionModule-int[]。*/
    /// </summary>
    public int[] FunctionModule
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
  /**获取chapterEvents-int[][]。*/
    /// </summary>
    public int[][] ChapterEvents
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
  /**获取chapterProgress-int[]。*/
    /// </summary>
    public int[] ChapterProgress
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
  /**获取dropRewardShow-int[]。*/
    /// </summary>
    public int[] DropRewardShow
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
  /**获取randomSeeds-string[][]。*/
    /// </summary>
    public string[][] RandomSeeds
    {
        get;
        private set;
    }

    /// <summary>
  /**获取releaseTime-int[][]。*/
    /// </summary>
    public int[][] ReleaseTime
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

    /// <summary>
  /**获取campLimit-int[]。*/
    /// </summary>
    public int[] CampLimit
    {
        get;
        private set;
    }

    /// <summary>
  /**获取bossSeed-int[]。*/
    /// </summary>
    public int[] BossSeed
    {
        get;
        private set;
    }

    /// <summary>
  /**获取seedMaxLimit-int[]。*/
    /// </summary>
    public int[] SeedMaxLimit
    {
        get;
        private set;
    }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] columnStrings = CSVSerializer.ParseCSVCol(dataRowString);

        int index = 0;
        CanInvest = DataTableParseUtil.ParseBool(columnStrings[index++]);
        ExtraDropPool = DataTableParseUtil.ParseInt(columnStrings[index++]);
        _id = int.Parse(columnStrings[index++]);
        IsRanking = DataTableParseUtil.ParseBool(columnStrings[index++]);
        Level = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        LoadingBg = DataTableParseUtil.ParseString(columnStrings[index++]);
        Name = DataTableParseUtil.ParseString(columnStrings[index++]);
        RefreshRatio = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ReputationRequaireScore = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RespawnTimes = DataTableParseUtil.ParseInt(columnStrings[index++]);
        RewardFrequency = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SceneName = DataTableParseUtil.ParseString(columnStrings[index++]);
        SceneSubtype = DataTableParseUtil.ParseInt(columnStrings[index++]);
        SceneType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        TalentType = DataTableParseUtil.ParseInt(columnStrings[index++]);
        WorldDropProbability = DataTableParseUtil.ParseInt(columnStrings[index++]);
        FunctionModule = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        BaseScore = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        BossCovers = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChallengeMode = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterBaseReward = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterEvents = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        ChapterLuckyReward = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterProgress = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        ChapterPunish = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        ChapterTimeLimit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        Chapters = DataTableParseUtil.ParseArray<string>(columnStrings[index++]);
        Desc = DataTableParseUtil.ParseString(columnStrings[index++]);
        DropRewardShow = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        PunishDesc = DataTableParseUtil.ParseString(columnStrings[index++]);
        RandomSeeds = DataTableParseUtil.ParseArrayList<string>(columnStrings[index++]);
        ReleaseTime = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        Tickets = DataTableParseUtil.ParseArrayList<int>(columnStrings[index++]);
        CampLimit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        BossSeed = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);
        SeedMaxLimit = DataTableParseUtil.ParseArray<int>(columnStrings[index++]);

        return true;
    }


    public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
    {
        using (MemoryStream memoryStream = new(dataRowBytes, startIndex, length, false))
        {
            using (BinaryReader binaryReader = new(memoryStream, Encoding.UTF8))
            {
                CanInvest = binaryReader.ReadBoolean();
                ExtraDropPool = binaryReader.Read7BitEncodedInt32();
                _id = binaryReader.Read7BitEncodedInt32();
                IsRanking = binaryReader.ReadBoolean();
                Level = binaryReader.ReadArray<Int32>();
                LoadingBg = binaryReader.ReadString();
                Name = binaryReader.ReadString();
                RefreshRatio = binaryReader.ReadArray<Int32>();
                ReputationRequaireScore = binaryReader.Read7BitEncodedInt32();
                RespawnTimes = binaryReader.Read7BitEncodedInt32();
                RewardFrequency = binaryReader.Read7BitEncodedInt32();
                SceneName = binaryReader.ReadString();
                SceneSubtype = binaryReader.Read7BitEncodedInt32();
                SceneType = binaryReader.Read7BitEncodedInt32();
                TalentType = binaryReader.Read7BitEncodedInt32();
                WorldDropProbability = binaryReader.Read7BitEncodedInt32();
                FunctionModule = binaryReader.ReadArray<Int32>();
                BaseScore = binaryReader.ReadArray<Int32>();
                BossCovers = binaryReader.ReadArray<Int32>();
                ChallengeMode = binaryReader.ReadArray<Int32>();
                ChapterBaseReward = binaryReader.ReadArray<Int32>();
                ChapterEvents = binaryReader.ReadArrayList<Int32>();
                ChapterLuckyReward = binaryReader.ReadArray<Int32>();
                ChapterProgress = binaryReader.ReadArray<Int32>();
                ChapterPunish = binaryReader.ReadArrayList<Int32>();
                ChapterTimeLimit = binaryReader.ReadArray<Int32>();
                Chapters = binaryReader.ReadArray<String>();
                Desc = binaryReader.ReadString();
                DropRewardShow = binaryReader.ReadArray<Int32>();
                PunishDesc = binaryReader.ReadString();
                RandomSeeds = binaryReader.ReadArrayList<String>();
                ReleaseTime = binaryReader.ReadArrayList<Int32>();
                Tickets = binaryReader.ReadArrayList<Int32>();
                CampLimit = binaryReader.ReadArray<Int32>();
                BossSeed = binaryReader.ReadArray<Int32>();
                SeedMaxLimit = binaryReader.ReadArray<Int32>();
            }
        }

        return true;
    }
}

