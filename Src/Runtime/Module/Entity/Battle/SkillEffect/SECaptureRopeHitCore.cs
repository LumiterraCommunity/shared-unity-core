
/// <summary>
/// 捕获绳击中
/// </summary>
public class SECaptureRopeHitCore : SkillEffectBase
{

    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }
        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
        RefEntity.EntityEvent.EntityBeHit?.Invoke(SkillID);
    }
}