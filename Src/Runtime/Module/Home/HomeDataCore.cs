using UnityEngine;
using static HomeDefine;

/// <summary>
/// 家园数据core
/// </summary>
public class HomeDataCore : MonoBehaviour
{
    /// <summary>
    /// 家园主人的用户id
    /// </summary>
    /// <value></value>
    public long OwnerPlayerId { get; private set; }
    public string OwnerPlayerName { get; private set; } = "-";

    /// <summary>
    /// 家园类型
    /// </summary>
    /// <value></value>
    public eHomeType HomeType { get; private set; }

    public int UsedSoilFertileValue { get; private set; }
    public int MaxSoilFertileValue { get; private set; }
    public int UsedAnimalHappyValue { get; private set; }
    public int MaxAnimalHappyValue { get; private set; }

    protected bool SoilFertileIsInit { get; private set; }
    protected bool AnimalHappyIsInit { get; private set; }

    /// <summary>
    /// 是否自己个人家园 个人家园不是一定是自己的家园
    /// </summary>
    public bool IsPersonalHome => HomeType == eHomeType.Personal;

    public void SetHomeType(eHomeType homeType)
    {
        HomeType = homeType;
    }

    public void SetOwnerInfo(long ownerUserId, string ownerName)
    {
        OwnerPlayerId = ownerUserId;
        OwnerPlayerName = ownerName;
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDestroy()
    {
    }

    public virtual void SetUsedSoilFertileValue(int fertileValue, bool init)
    {
        UsedSoilFertileValue = fertileValue;

        if (init)
        {
            SoilFertileIsInit = true;
        }
    }

    public virtual void SetUsedAnimalHappyValue(int happyValue, bool init)
    {
        UsedAnimalHappyValue = happyValue;

        if (init)
        {
            AnimalHappyIsInit = true;
        }
    }

    public virtual void SetMaxFertileAndHappyValue(int maxFertile, int maxHappy)
    {
        MaxSoilFertileValue = maxFertile;
        MaxAnimalHappyValue = maxHappy;
    }

    /// <summary>
    /// 剩余的土地肥沃值 >= 0
    /// </summary>
    /// <returns></returns>
    public int GetRemainSoilFertileValue()
    {
        return Mathf.Max(0, MaxSoilFertileValue - UsedSoilFertileValue);
    }

    /// <summary>
    /// 剩余的动物快乐值 >= 0
    /// </summary>
    /// <returns></returns>
    public int GetRemainAnimalHappyValue()
    {
        return Mathf.Max(0, MaxAnimalHappyValue - UsedAnimalHappyValue);
    }
}