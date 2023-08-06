using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportEndSequence : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float initialDelay = .2f;

    bool sequenceExcecuted = false;
    public void TrySequence()
    {
        if (sequenceExcecuted) return;

        GameManager.instance.EnableControls(false);
        StartCoroutine(WalkToSequence());

        IEnumerator WalkToSequence()
        {
            HeroManager.instance.SetHeroDestination(transform.position);
            yield return new WaitForSeconds(.2f);
            yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
            StartSequence();
        }

    }

    void StartSequence()
    {
        Transform heroTransform = HeroVisual.instance.transform;
        sequenceExcecuted = true;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(initialDelay);
        // face forward
        sequence.Append(heroTransform.DORotate(Vector3.up * 180, 1));
        // animate up
        sequence.AppendCallback(() => HeroVisual.instance.TriggerHeroAnimation("teleport"));
        sequence.Append(heroTransform.DOMoveY(10,5));
        sequence.AppendCallback(()=> GameEndingScreen.instance.ShowEndingScreen());

    }
}
