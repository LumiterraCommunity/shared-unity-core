/// <summary>
/// 种子实体业务侧初始化逻辑 只有装配了实现该接口的组件都会被初始化 一个实体可以有多个这样的组件都会初始化
/// </summary>
public interface ISeedEntityInitLogic
{
    /// <summary>
    /// 组件装配完成后 正式业务侧的初始化逻辑
    /// </summary>
    /// <param name="SeedEntityCore"></param>
    void LogicInit(SeedEntityCore SeedEntityCore);
}