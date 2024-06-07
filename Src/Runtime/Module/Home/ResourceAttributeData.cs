using GameMessageCore;

public class ResourceAttributeData : EntityAttributeData
{
    protected override AttributeData GenerateOneNetAttribute(eAttributeType type, IntAttribute attribute)
    {
        return new AttributeData()
        {
            Id = (int)type,
            Value = attribute.Value,
        };
    }
}