/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 许愿池元素
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/WishPoolElementCore.cs
 * 
 */

using System.Collections;
using UnityEngine;

public class WishPoolElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.WishPool;

    [Header("许愿效果")]
    public GameObject Prefab;

    [Header("持续时间")]
    public float Duration;

    protected GameObject CurParticleObject;
    public void PlayEffect()
    {
        if (Prefab == null)
        {
            return;
        }
        if (CurParticleObject != null)
        {
            return;
        }
        _ = StartCoroutine(PlayAndStopParticle());

    }

    private IEnumerator PlayAndStopParticle()
    {
        PlayParticle();
        yield return new WaitForSeconds(Duration);
        StopParticle();
    }

    private void PlayParticle()
    {
        CurParticleObject = Instantiate(Prefab);
        CurParticleObject.transform.parent = transform;
        CurParticleObject.transform.localPosition = Prefab.transform.localPosition;
        if (CurParticleObject.TryGetComponent(out ParticleSystem particleSystem))
        {
            particleSystem.Play(true);
        }
    }
    private void StopParticle()
    {
        Destroy(CurParticleObject);
        CurParticleObject = null;
    }
}
