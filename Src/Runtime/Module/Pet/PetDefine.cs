/// <summary>
/// 宠物特性 位运算
/// </summary>
public enum ePetFeature
{
    None = 0,
    Attack = 1 << 0,//攻击
    Collect = 1 << 1,//采集
    Farming = 1 << 2,//农耕
    Mount = 1 << 3,//坐骑
    Support = 1 << 4,//支援
    Skill = 1 << 5,//有特殊技能
}