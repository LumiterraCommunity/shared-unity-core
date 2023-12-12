/// <summary>
/// 掉落死亡检测
/// </summary>
public class FallDeathDetectionCore : EntityBaseComponent, ISceneDamageDetection
{
    public const float DEATH_HEIGHT = -100;//死亡层高度坐标
    public static int Detection_interval = 10;// 检测间隔 单位帧 为了优化 不需要实时那么准确
    private int _count = 0;

    private void Start()
    {
        StartDetection();
    }

    public void StartDetection()
    {
        enabled = true;
        _count = 0;
    }

    public void StopDetection()
    {
        enabled = false;
    }

    private void Update()
    {
        _count++;
        if (_count < Detection_interval)
        {
            return;
        }

        _count = 0;

        if (transform.position.y < DEATH_HEIGHT)
        {
            StopDetection();

            if (RefEntity.BattleDataCore.IsLive())//保护 理论上会禁止update检测 直到下次重新开始检测
            {
                OnFallDeath();
            }
        }
    }

    private void OnFallDeath()
    {
        RefEntity.EntityEvent.OnSceneDeath?.Invoke(GameMessageCore.DamageState.Fall);
    }
}