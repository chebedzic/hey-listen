using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using DG.Tweening;

public class BrazierPuzzleHandler : MonoBehaviour
{
    public bool groupFire = false;
    public bool onFire;

    public UnityEvent<bool> OnSetFire;
    [ShowIf("groupFire")]
    public float cooldownInterval = 1;
    [ShowIf("groupFire")]
    public UnityEvent OnMultipleFire;
    [ShowIf("groupFire")]
    public ParticleSystem[] fireParticles;
    public PropFireAnimation[] props;
    public AudioClipContainer litSound;

    public void JumpInteraction()
    {
        if (HeroManager.instance.heroIsOnFire)
        {
            SetFire(true);

            if (groupFire)
            {
                StartCoroutine(Cooldown());
                IEnumerator Cooldown()
                {
                    yield return new WaitForSeconds(cooldownInterval);
                    SetFire(false);
                }
            }
        }

        if (onFire)
        {
            if(!HeroManager.instance.heroIsOnFire)
                AudioManager.instance.PlaySFX(litSound, null);
            HeroManager.instance.SetEquipmentState(true, true);
        }
    }
    public void HandleInteraction()
    {
        if (HeroManager.instance.equipmentIsOnFire)
        {
            SetFire(true);

            if (groupFire)
            {
                StartCoroutine(Cooldown());
                IEnumerator Cooldown()
                {
                    yield return new WaitForSeconds(cooldownInterval);
                    SetFire(false);
                }
            }
        }

        if (onFire)
        {
            if (!HeroManager.instance.equipmentIsOnFire)
                AudioManager.instance.PlaySFX(litSound, null);
            HeroManager.instance.SetEquipmentState(true);
        }

    }

    public void HandleMultipleInteractions()
    {
        if(HeroManager.instance.equipmentIsOnFire)
        {
            MultipleSetFire(true);
        }

        if (onFire)
        {
            HeroManager.instance.SetEquipmentState(true);
        }

    }

    public void SetFire(bool fire)
    {
        if(fire)
            AudioManager.instance.PlaySFX(litSound, null);
        onFire = fire;
        OnSetFire.Invoke(fire);
    }

    public void MultipleSetFire(bool fire)
    {
        onFire = fire;
        if (fire)
        {
            OnMultipleFire.Invoke();

            HeroVisual.instance.TurnSmearFireOn(true);

            Sequence s = DOTween.Sequence();
            for (int i = 0; i < fireParticles.Length; i++)
            {
                int index = i;
                s.AppendInterval(.2f);
                //s.AppendCallback(() => fireParticles[index].gameObject.SetActive(true));
                s.AppendCallback(() => props[index].SetFire(true));
                s.AppendCallback(() => AudioManager.instance.PlaySFX(litSound, null));
            }
            s.AppendInterval(.5f);
            s.AppendCallback(() => GetComponent<InteractablePuzzle>().PuzzleSolved());
            s.AppendCallback(() => HeroVisual.instance.TurnSmearFireOn(false));
        }

    }



}
