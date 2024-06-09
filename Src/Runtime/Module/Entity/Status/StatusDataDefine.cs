/// <summary>
/// 状态上流转的数据统一定义
/// </summary>
public static class StatusDataDefine
{
    public const string SKILL_INPUT = "SKILL_INPUT";
    public const string SKILL_START_POS = "SKILL_START_POS";
    public const string BE_HIT_MOVE_TIME = "BE_HIT_MOVE_TIME";
    public const string CAPTURE_FROM_ID = "CAPTURE_FROM_ID";

    #region 游戏主流程状态机
    public const string FROM_LOGIN = "FROM_LOGIN";
    public const string TARGET_SCENE_NAME = "TARGET_SCENE_NAME";
    public const string LAST_SCENE_NAME = "LAST_SCENE_NAME";
    #endregion 
}