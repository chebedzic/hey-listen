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
                s.AppendCallback(() => fireParticles[index].gameObject.SetActive(true));
            }
            s.AppendInterval(.5f);
            s.AppendCallback(() => GetComponent<InteractablePuzzle>().PuzzleSolved());
            s.AppendCallback(() => HeroVisual.instance.TurnSmearFireOn(false));
        }

    }



}
