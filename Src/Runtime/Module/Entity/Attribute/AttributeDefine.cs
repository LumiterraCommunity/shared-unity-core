/*
 * @Author: xiang huan
 * @Date: 2023-01-11 14:44:39
 * @Description: 属性定义
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/AttributeDefine.cs
 * 
 */

/// <summary>
/// 属性修改器类型
/// </summary>
public enum eModifierType : int
{
    Add = 1,  //增加
    PctAdd,   //百分比增加
    FinalAdd,  //最终增加
    FinalPctAdd,  //最终百分比增加
}

/// <summary>
/// 属性类型
/// </summary>
public enum eAttributeValueType : int
{
    Int = 1,  //整型
    ThousandthPct = 2,   //千分位百分比百分比
    Thousandth = 3,  //千分位浮点
}