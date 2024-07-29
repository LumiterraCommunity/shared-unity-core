/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 许愿池元素
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/WishPoolElementCore.cs
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishPoolElementCore : SceneElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.WishPool;

    [Header("许愿效果")]
    public GameObject Prefab;

    [Header("持续时间")]
    public float Duration;
    [Header("抽奖卷列表")]
    public int[] RaffleTicketList;
    private readonly HashSet<int> _raffleTicketSet = new();
    protected GameObject CurParticleObject;

    protected override void Awake()
    {
        base.Awake();
        if (RaffleTicketList != null)
        {
            for (int i = 0; i < RaffleTicketList.Length; i++)
            {
                _ = _raffleTicketSet.Add(RaffleTicketList[i]);
            }
        }
    }
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
    //是否包含在许愿卷列表中
    public bool IsRaffleTicket(int cid)
    {
        return _raffleTicketSet.Contains(cid);
    }
}
