using UnityEngine;

/// <summary>
/// 畜牧动物掉落物core
/// </summary>
public abstract class AnimalDropCore : MonoBehaviour
{
    protected AnimalProductSaveData RefProductSaveData { get; private set; }
    protected ulong AnimalId { get; private set; }
    public virtual void InitAnimalDrop(AnimalProductSaveData productSaveData, ulong animalId)
    {
        RefProductSaveData = productSaveData;
        AnimalId = animalId;
    }
}