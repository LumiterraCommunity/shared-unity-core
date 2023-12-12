/// <summary>
/// 用来处理UID相关的工具
/// </summary>
public static class UIDUtil
{
    /// <summary>
    /// 一个对象中需要多个UID时 将转成Long类型的UID
    /// </summary>
    /// <param name="classObject">类对象</param>
    /// <param name="extraObject">类中为了区分的其他对象 可以是数字或者字符串等等</param>
    /// <returns></returns>
    public static long ToLongUID(object classObject, object extraObject)
    {
        long longUID = ((long)classObject.GetHashCode() << 32) | (uint)extraObject.GetHashCode();
        return longUID;
    }
}