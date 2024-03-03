using UnityEngine;
public class EntityBaseData : EntityBaseComponent
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private GameMessageCore.EntityType _type;

    public long Id => _id;
    public GameMessageCore.EntityType Type => _type;

    public void Init(long id, GameMessageCore.EntityType type)
    {
        _id = id;
        _type = type;
    }

    public void Reset()
    {
        _id = 0;
        _type = GameMessageCore.EntityType.All;
    }
}