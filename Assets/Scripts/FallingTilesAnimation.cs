using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UIElements;

public class FallingTilesAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] tileRows;

    [Header("Settings")]
    [SerializeField] private float positionFallAmount = -3;
    [SerializeField] private float fallDuration = .3f;
    [SerializeField] private float fallDelay = .2f;

    public void TriggerAnimation(bool startsLeft = true)
    {

        for (int i =  0 ; i < tileRows.Length; i++)
        {
            int index = startsLeft ? i : tileRows.Length - i - 1;
            int reverseIndex = startsLeft ? tileRows.Length - i - 1 : i;
            tileRows[index].DOComplete();
            tileRows[index].DOMoveY(positionFallAmount, fallDuration).SetDelay(fallDelay * (startsLeft ? index + 1 : reverseIndex + 1))
                .OnStart(()=> tileRows[index].GetComponentInChildren<ParticleSystem>().Play());

        }
    }

    public void ResetAnimation()
    {
        for (int i = 0; i < tileRows.Length; i++)
        {
            tileRows[i].DOComplete();
            tileRows[i].localPosition = Vector3.zero;
        }
    }


}
