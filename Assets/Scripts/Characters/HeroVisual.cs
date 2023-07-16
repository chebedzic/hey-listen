using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroVisual : MonoBehaviour
{

    private Animator animator;
    private HeroManager heroManager;
    [SerializeField] private ParticleSystem heroHitParticle;
    [SerializeField] private ParticleSystem enemyHitParticle;
    [SerializeField] [ColorUsage(true,true)] private Color enemyHitColor;
    private Renderer[] renderers;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        heroManager = GetComponentInParent<HeroManager>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        animator.SetFloat("velocity", heroManager.IsAgentCrossingLink() ? 2 : heroManager.GetHeroVelocity().Remap(0,heroManager.GetHeroSpeed(),0,2));
    }
    public void HitByEnemy()
    {
        transform.DOComplete();
        transform.DOShakeScale(.2f, .5f, 20, 90, true);
        TriggerHeroAnimation("death");
        PlayHitParticle(true);
        BlinkEmission();
    }

    public void TriggerHeroAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }


    public void ActivateHeroAnimation(string name, bool state)
    {
        animator.SetBool(name, state);
    }

    public void PlayHitParticle(bool enemy)
    {
        ParticleSystem part = enemy ? enemyHitParticle : heroHitParticle;
        part.Play();
    }

    public void BlinkEmission()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.DOComplete();
            renderer.material.SetColor("_FresnelColor", enemyHitColor);
            renderer.material.DOFloat(1, "_FresnelAmount", .1f).OnComplete(() => renderer.material.DOFloat(0, "_FresnelAmount", .2f));
        }
    }


}
