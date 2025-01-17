using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class TeleportEndSequence : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float initialDelay = .2f;
    [SerializeField] private Transform companionPosReference;
    public UnityEvent OnSequenceStart;

    public Volume endEffect;
    public float endEffectDuration = 5;
    public float endEffectDelay = 2;

    bool sequenceExcecuted = false;
    public void TrySequence()
    {
        if (sequenceExcecuted) return;

        GameManager.instance.EnableControls(false);
        CompanionManager.instance.transform.DORotate(new Vector3(0, 180, 0), 1, RotateMode.Fast);
        CompanionManager.instance.transform.DOMove(companionPosReference.position, 1).OnComplete(()=> CompanionManager.instance.transform.parent = companionPosReference);
        StartCoroutine(WalkToSequence());

        IEnumerator WalkToSequence()
        {
            HeroManager.instance.SetHeroDestination(transform.position);
            yield return new WaitForSeconds(.2f);
            yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
            StartSequence();
            DOVirtual.Float(0, 1, endEffectDuration, SetEndEffect).SetDelay(endEffectDelay);
        }

    }

    void StartSequence()
    {
        OnSequenceStart.Invoke();
        HeroVisual.instance.ActivateRunParticle(false);

        Transform heroTransform = HeroVisual.instance.transform;
        sequenceExcecuted = true;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(initialDelay);
        sequence.AppendCallback(() => AudioManager.instance.SetMusicVolume(-80, .5f));
        // face forward
        sequence.Append(heroTransform.DORotate(Vector3.up * 180, 1));
        // animate up
        sequence.AppendCallback(() => HeroVisual.instance.TriggerHeroAnimation("teleport"));
        sequence.Append(heroTransform.DOMoveY(10,5));
        sequence.AppendCallback(()=> GameEndingScreen.instance.ShowEndingScreen());

    }

    void SetEndEffect(float amount)
    {
        if(endEffect!= null)
        endEffect.weight = amount;
    }
}
