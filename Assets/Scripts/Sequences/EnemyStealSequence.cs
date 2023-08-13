using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStealSequence : MonoBehaviour
{
    public Action requiredCompanionAction;
    public Action requiredSlotAction;
    public InteractableCollectable collectableToDisable;
    public GameObject enemyGameobject;
    public Transform cameraFocusPoint;

    [Header("Parameters")]
    [SerializeField] private float initialDelay = .2f;
    [SerializeField] private float cameraTransition = .7f;
    [SerializeField] AudioClipContainer enemyNoticeAudio;
    [SerializeField] AudioClipContainer enemyJumpAudio;

    bool sequenceExcecuted = false;

    public void TrySequence()
    {
        if (sequenceExcecuted) return;
        if(CompanionManager.instance.heldAction == requiredCompanionAction && collectableToDisable.collectableAction == requiredSlotAction)
        {
            StartSequence();
        }
    }

    void StartSequence()
    {
        sequenceExcecuted = true;

        collectableToDisable.collectableAction = null;
        collectableToDisable.gameObject.SetActive(false);
        enemyGameobject.SetActive(true);

        Transform enemyT = enemyGameobject.transform;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(initialDelay);
        sequence.AppendCallback(()=> GameManager.instance.EnableControls(false));
        sequence.AppendCallback(() => GameManager.instance.FocusCameraOnObject(cameraFocusPoint, true, cameraTransition));
        sequence.AppendInterval(cameraTransition);
        sequence.Append(enemyT.DOLookAt(HeroManager.instance.transform.position, .5f, AxisConstraint.Y));
        sequence.AppendCallback(() => AudioManager.instance.PlaySFX(enemyNoticeAudio, null));
        sequence.Append(enemyT.DOJump(enemyT.position, 1, 1, .5f, false));
        sequence.Append(enemyT.DOLookAt(enemyT.position + Vector3.forward, .5f, AxisConstraint.Y));
        sequence.AppendCallback(() => AudioManager.instance.PlaySFX(enemyJumpAudio, null));
        sequence.Append(enemyT.DOJump(enemyT.position + (Vector3.forward * 10), 15, 1, 1));
        sequence.AppendCallback(()=> GameManager.instance.EnableControls(true));
        sequence.AppendCallback(() => GameManager.instance.FocusCameraOnObject(cameraFocusPoint, false, cameraTransition));
        sequence.AppendCallback(() => enemyGameobject.SetActive(false));
    }
}
