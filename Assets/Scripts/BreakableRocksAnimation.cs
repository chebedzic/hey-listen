using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakableRocksAnimation : MonoBehaviour
{
    [SerializeField] private float initialDelay = .2f;
    [SerializeField] private float breakDelay = .1f;
    [SerializeField] private Transform[] rockRows;
    [SerializeField] private ParticleSystem[] particles;

    public void TriggerAnimation()
    {

        for (int i = 0; i < rockRows.Length; i++)
        {
            int index = i;
            rockRows[index].DOScale(0, 0).SetDelay(initialDelay + (breakDelay * index)).OnStart(() => particles[index].Play());

        }
    }
}
