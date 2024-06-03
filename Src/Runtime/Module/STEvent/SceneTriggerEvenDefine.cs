/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:57:19
 * @Description: 
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/SceneTriggerEvenDefine.cs
 * 
 */

public static class SceneTriggerEvenDefine
{
    public const int CONFIG_SKILL_ID = 14;   // 配置技能ID

}


public enum eSTEventType : int
{
    Main = 1, //主事件
}
public enum eSTConditionType : int
{
    None = 0,
    STCCheckLevelScoreProgress = 1, //检测关卡分进度

}

public enum eSTActionType : int
{
    None = 0,
    STARandomSelector = 1, //权重随机选择
    STAAddSKillEffect = 2, //实体添加技能效果
    STAHomeAttributeModify = 3, //家园对象属性修改
    STASoilBehavior = 4, //作物行为
}

public enum eSTConditionCheckType : int
{
    And = 1,
    Or = 2,
}
public enum eSTCCheckLevelScoreType : int
{
    Normal = 1, //普通
    Percent = 2, //百分比
}
public enum eSTConditionComparison : int
{
    Equal = 1, //等于
    NotEqual = 2, //不等于
    Greater = 3, //大于
    GreaterEqual = 4, //大于等于
    Less = 5, //小于
    LessEqual = 6, //小于等于
}