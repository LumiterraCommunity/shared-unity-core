/*
 * @Author: xiang huan
 * @Date: 2023-02-09 14:54:09
 * @Description: 根据随机种子，固定计算随机值
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/Skill/InputRandomData.cs
 * 
 */

public class InputRandomData
{
    public int Seed { get; private set; }
    /// <summary>
    /// 命中随机值
    /// </summary>
    public int HitValue { get; private set; }
    /// <summary>
    /// 暴击随机值
    /// </summary>
    public int CritValue { get; private set; }
    private System.Random _random;
    public void SetInputRandomSeed(int seed)
    {
        _random = new System.Random(seed);
        Seed = seed;
        HitValue = _random.Next(0, 100);
        CritValue = _random.Next(0, 1000);
    }
}