using GameMessageCore;

/// <summary>
/// 土地上的属性管理组件
/// </summary>
public class SoilAttributeData : AttributeDataCpt
{
    private SoilEvent _soilEvent;

    protected void Start()
    {
        _soilEvent = GetComponent<SoilEvent>();
    }

    protected override void OnAttributeUpdate(eAttributeType type, IntAttribute attribute)
    {
        base.OnAttributeUpdate(type, attribute);

        if (_soilEvent != null)//初始化时不需要广播
        {
            _soilEvent.OnAttributeUpdated?.Invoke(type, attribute.Value);
        }
    }

    protected override AttributeData GenerateOneNetAttribute(eAttributeType type, IntAttribute attribute)
    {
        return new AttributeData()
        {
            Id = (int)type,
            Value = attribute.Value,
        };
    }
}