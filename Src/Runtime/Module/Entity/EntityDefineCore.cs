/// <summary>
/// 实体的一些定义 共享库
/// </summary>
public static class EntityDefineCore
{
    /// <summary>
    /// 实体的保护等级 稍微大点 防止配置异常时需要给实体等级默认值可以用这个 不要用0 因为0的属性很低可以轻松刷 防止配置异常资源被刷漏洞
    /// </summary>
    public const float PROTECT_LEVEL = 99;
}