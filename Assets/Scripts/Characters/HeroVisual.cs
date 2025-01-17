using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    public static HeroVisual instance;

    private Animator animator;
    private HeroManager heroManager;
    private Renderer[] renderers;

    [Header("Particles")]
    [SerializeField] private ParticleSystem heroHitParticle;
    [SerializeField] private ParticleSystem enemyHitParticle;
    [SerializeField] private ParticleSystem doorDestroyParticle;
    [SerializeField] [ColorUsage(true,true)] private Color enemyHitColor;
    [SerializeField] private ParticleSystem confusionParticle;
    [SerializeField] private ParticleSystem equipmentTutorialParticle;
    [SerializeField] private ParticleSystem runParticle;

    [Header("Equipment References")]
    [SerializeField] private GameObject swordGameobject;
    [SerializeField] private GameObject shieldGameobject;
    [SerializeField] private Transform equipmentRigPosition;
    [SerializeField] [ColorUsage(true, true)] Color equipmentBlinkColor;
    private Renderer[] equipmentRenderers;
    [SerializeField] private ParticleSystem[] fireParticles;
    [SerializeField] private ParticleSystem heroFireParticle;
    [SerializeField] private ParticleSystem smearFireParticle;
    public ParticleSystem newEquipmentParticle;

    [Header("Confusion Settings")]
    [SerializeField] private float confusionBackoutInterval = 1;
    [SerializeField] private Vector3 confusionParticleOffset = new Vector3(1, 0, -.5f);

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialCursorIndicator;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        heroManager = GetComponentInParent<HeroManager>();
        equipmentRenderers = swordGameobject.transform.parent.GetComponentsInChildren<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();
        heroManager.OnGetEquipment.AddListener(EquipmentVisual);
        heroManager.OnEquipmentFire.AddListener(EquipmentFire);
        heroManager.OnHeroFire.AddListener(HeroFire);

        EquipmentVisual(heroManager.currentEquipment);
    }

    void Update()
    {
        animator.SetFloat("velocity", heroManager.IsAgentCrossingLink() ? 2 : heroManager.GetHeroVelocity().Remap(0,heroManager.GetHeroSpeed(),0,2));
    }

    public void EquipmentVisual(Equipment equipment)
    {
        if (equipment == null)
        {
            SetEquipmentVisibility(gameObject);
            return;
        }

        if (equipment != null)
        {
            transform.parent.eulerAngles = new Vector3(0, 180, 0);
            animator.SetTrigger("equip");
        }

        switch (equipment.type)
        {
            case EquipmentType.sword:
                SetEquipmentVisibility(swordGameobject);
                break;
            case EquipmentType.shield:
                SetEquipmentVisibility(shieldGameobject);
                break;
            default:
                break;
        }
    }

    public void EquipmentFire(bool fire)
    {
        foreach(ParticleSystem part in fireParticles)
        {
            if (fire) part.Play();
            else part.Stop();
        }
    }

    public void HeroFire(bool fire)
    {
        if (fire) heroFireParticle.Play();
        else heroFireParticle.Stop();
    }

    void SetEquipmentVisibility(GameObject equipment)
    {
        shieldGameobject.SetActive(equipment == shieldGameobject);
        swordGameobject.SetActive(equipment == swordGameobject);

        equipmentRigPosition.DOComplete();
        equipmentRigPosition.DOShakeScale(.3f, .8f,15, 90, true);

        foreach (Renderer renderer in equipmentRenderers)
        {
            renderer.material.SetColor("_FresnelColor", equipmentBlinkColor);
            renderer.material.DOComplete();
            renderer.material.DOFloat(1, "_FresnelAmount", .1f).OnComplete(() => renderer.material.DOFloat(0, "_FresnelAmount", .4f));
        }

    }

    public void HitByEnemy()
    {
        transform.DOComplete();
        transform.DOShakeScale(.2f, .5f, 20, 90, true);
        TriggerHeroAnimation("death");
        PlayHitParticle(true);
        BlinkEmission();
    }

    public void PlayConfusedParticle()
    {
        Vector3 heroPos = heroManager.transform.position;

        confusionParticle.transform.position = 
            new Vector3(heroPos.x + confusionParticleOffset.x, confusionParticle.transform.position.y, heroPos.z - confusionParticleOffset.z);

        Sequence confusedSequence = DOTween.Sequence()
            .AppendCallback(() => confusionParticle.Play())
            .AppendCallback(() => AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Confusion, null))
            .AppendInterval(confusionBackoutInterval)
            .AppendCallback(() => 
            { 
                if(heroManager.AgentIsStopped() && !heroManager.IsAgentCrossingLink() && !heroManager.isLookingForBridge && !heroManager.isInteracting)
                    heroManager.SetHeroDestination(heroManager.transform.position + (heroManager.transform.forward * -1));
            });
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

    public void PlayDoorDestroyParticle(Vector3 pos)
    {
        doorDestroyParticle.transform.position = pos;
        doorDestroyParticle.Play();
    }

    public void BlinkEmission()
    {
        foreach (Renderer renderer in renderers)
        {
            if (!renderer.material.HasFloat("_FresnelAmount"))
                break;

            Color storeColor = renderer.material.GetColor("_FresnelColor");

            renderer.material.DOComplete();
            renderer.material.SetColor("_FresnelColor", enemyHitColor);
            renderer.material.DOFloat(1, "_FresnelAmount", .1f).OnComplete(()=>CompleteBlink(renderer, storeColor));
        }
    }

    public void SetEquipmentTutorial(bool state)
    {
        heroManager.canMove = !state;
        animator.SetBool("isConfused", state);
        tutorialCursorIndicator.SetActive(state);
        if (state) equipmentTutorialParticle.Play(); else equipmentTutorialParticle.Stop();
    }

    public void ActivateRunParticle(bool activate)
    {
        if (activate) runParticle.Play();
        else runParticle.Stop();
    }

    public void TurnSmearFireOn(bool active)
    {
        smearFireParticle.gameObject.SetActive(active);
    }

    void CompleteBlink(Renderer renderer, Color color)
    {
        renderer.material.DOFloat(0, "_FresnelAmount", .2f);
        renderer.material.SetColor("_FresnelColor", color);
    }


}
