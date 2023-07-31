using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class BlockFadeAnimation : MonoBehaviour
{
    Renderer[] childRenderers;
    [SerializeField] private float fadeDuration;
    [SerializeField] private ParticleSystem fadeParticle;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    bool fade = false;

    // Start is called before the first frame update
    void Awake()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
                renderer.material.SetFloat("_FrenselAmount", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade(bool state)
    {

        foreach (Renderer renderer in childRenderers)
        {
            if (state)
            {
                renderer.transform.DOScale(0, fadeDuration/2).From().SetEase(Ease.InBack);
                renderer.material.DOFloat(0, "_FresnelAmount", fadeDuration);
            }
            else
            {
                renderer.transform.DOScale(0, fadeDuration/2).SetEase(Ease.OutSine).SetDelay(fadeDuration/2);
                renderer.material.DOFloat(1, "_FresnelAmount", fadeDuration);
            }
        }
        fade = state;
        Invoke("Complete", fade ? 0 : fadeDuration);
    }

    void Complete()
    {
        if(fadeParticle != null)
            fadeParticle.Play();

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        if(!fade)
            gameObject.SetActive(false);
    }
}
