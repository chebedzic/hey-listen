using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroVisual : MonoBehaviour
{

    private Animator animator;
    private HeroManager heroManager;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        heroManager = GetComponentInParent<HeroManager>();
    }

    void Update()
    {
        animator.SetFloat("velocity", heroManager.IsAgentCrossingLink() ? 2 : heroManager.GetHeroVelocity().Remap(0,heroManager.GetHeroSpeed(),0,2));
    }
}
