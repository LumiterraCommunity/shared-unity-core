/// <summary>
/// 家园实体业务侧初始化逻辑 只有装配了实现该接口的组件都会被初始化 一个实体可以有多个这样的组件都会初始化
/// </summary>
public interface IHomeEntityInitLogic
{
    /// <summary>
    /// 组件装配完成后 正式业务侧的初始化逻辑
    /// </summary>
    /// <param name="homeEntityCore"></param>
    void LogicInit(HomeEntityCore homeEntityCore);
}