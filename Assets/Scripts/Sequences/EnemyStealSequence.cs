using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStealSequence : MonoBehaviour
{
    public Action requiredCompanionAction;
    public InteractableCollectable collectableToDisable;
    public GameObject enemyGameobject;

    public void TrySequence()
    {
        if(CompanionManager.instance.heldAction == requiredCompanionAction)
        {
            StartSequence();
        }
    }

    void StartSequence()
    {
        collectableToDisable.collectableAction = null;
        collectableToDisable.gameObject.SetActive(false);
        enemyGameobject.SetActive(true);

        Transform enemyT = enemyGameobject.transform;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(.5f);
        sequence.AppendCallback(()=> GameManager.instance.EnableControls(false));
        sequence.AppendInterval(1);
        sequence.Append(enemyT.DOLookAt(HeroManager.instance.transform.position, .5f, AxisConstraint.Y));
        sequence.Append(enemyT.DOJump(enemyT.position, 1, 1, .5f, false));
        sequence.Append(enemyT.DOLookAt(enemyT.position + Vector3.forward, .5f, AxisConstraint.Y));
        sequence.Append(enemyT.DOJump(enemyT.position + (Vector3.forward * 20), 7, 1, 1));
        sequence.AppendCallback(()=> GameManager.instance.EnableControls(true));
    }
}
