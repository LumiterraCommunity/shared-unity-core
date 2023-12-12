/*
 * @Author: xiang huan
 * @Date: 2023-02-13 18:59:46
 * @Description: 技能效果存储数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Data/SkillEffectSaveData.cs
 * 
 */

public class SkillEffectSaveData
{
    /// <summary>
    /// fromID
    /// </summary>
    public long FromID;
    /// <summary>
    /// 效果销毁时间 ms
    /// </summary>
    public long DestroyTimestamp;
    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillID;
    /// <summary>
    /// 效果ID
    /// </summary>
    public int EffectID;
    /// <summary>
    /// 当前层级
    /// </summary>
    public int CurLayer;
    /// <summary>
    /// 下次间隔触发时间
    /// </summary>
    public long NextIntervalTime;
}