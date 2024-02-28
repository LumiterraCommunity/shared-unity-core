using UnityEngine;

/// <summary>
/// 畜牧动物掉落物core
/// </summary>
public abstract class AnimalDropCore : MonoBehaviour
{
    protected AnimalProductSaveData RefProductSaveData { get; private set; }
    protected long AnimalId { get; private set; }
    public virtual void InitAnimalDrop(AnimalProductSaveData productSaveData, long animalId)
    {
        RefProductSaveData = productSaveData;
        AnimalId = animalId;
    }
}