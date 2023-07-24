using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingTilesAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] tileRows;

    [Header("Settings")]
    [SerializeField] private float positionFallAmount = -3;
    [SerializeField] private float fallDuration = .3f;
    [SerializeField] private float fallDelay = .2f;

    public void TriggerAnimation(bool startsLeft = true)
    {
        for (int i = startsLeft ? 0 : tileRows.Length - 1; i < tileRows.Length; i = startsLeft ? i+1 : i-1)
        {
            tileRows[i].DOComplete();
            tileRows[i].DOMoveY(positionFallAmount, fallDuration).SetDelay(fallDelay * i);
        }
    }

    public void ResetAnimation()
    {
        for (int i = 0; i < tileRows.Length; i++)
        {
            tileRows[i].localPosition = Vector3.zero;
        }
    }


}
