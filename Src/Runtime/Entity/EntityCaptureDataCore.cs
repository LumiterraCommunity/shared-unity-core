


using GameMessageCore;
/// <summary>
/// 实体上的捕获数据 双端通用的核心数据
/// </summary>
public class EntityCaptureDataCore : EntityBaseComponent
{
    public CaptureStatus Status = CaptureStatus.Unknown;
    public long CaptureId { get; protected set; } = BattleDefine.ENTITY_ID_UNKNOWN;
    // 捕获开始时间
    public long CaptureAt { get; protected set; }

    // 捕获绳子血量
    public int RopeCurHp { get; protected set; }
    public readonly int RopeMaxHp = CaptureDefine.MAX_CAPTURE_ROPE_HP;

    // 捕获怪物血量
    public float CaptureCurHp { get; protected set; }
    public float CaptureMaxHp { get; protected set; }

    public float CaptureProgress => CaptureMaxHp > 0 ? CaptureCurHp / CaptureMaxHp : 0;

    private readonly CaptureData _netData = new();

    // 是否处于捕获状态（正在捕获，或 已被捕获）
    public bool IsInCaptureStatus()
    {
        return IsInCapturing() || Status is CaptureStatus.EndSuccess;
    }

    // 捕获中
    public bool IsInCapturing()
    {
        return Status is CaptureStatus.Start or CaptureStatus.Update;
    }

    public void SetStatus(CaptureStatus status)
    {
        Status = status;
    }

    public void ClearCaptureData()
    {
        CaptureId = BattleDefine.ENTITY_ID_UNKNOWN;
        CaptureAt = 0;
        RopeCurHp = 0;
        CaptureCurHp = 0;
        CaptureMaxHp = 0;
        Status = CaptureStatus.Unknown;
    }

    public CaptureData GetNetData()
    {
        _netData.CaptureId = CaptureId;
        _netData.CaptureAt = CaptureAt;
        _netData.CaptureCurHp = CaptureCurHp;
        _netData.CaptureMaxHp = CaptureMaxHp;
        _netData.RopeCurHp = RopeCurHp;
        _netData.RopeMaxHp = RopeMaxHp;
        _netData.Status = Status;
        return _netData;
    }
}