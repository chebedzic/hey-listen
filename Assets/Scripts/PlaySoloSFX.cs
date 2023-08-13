using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoloSFX : MonoBehaviour
{
    [SerializeField] private AudioClipContainer audioClip;
    [SerializeField] private float delay;

    public void PlaySFX()
    {
        if (audioClip != null)
            StartCoroutine(PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.PlaySFX(audioClip, null);
    }
}
