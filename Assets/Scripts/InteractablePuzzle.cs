using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InteractablePuzzle : Interactable
{

    [Header("Puzzle")]
    [SerializeField] private OffMeshLink offMeshLink;
    private StateMachine stateMachine;
    private InteractableModal linkedModal;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public void TryPuzzle(List<Action> actionList, InteractableModal modal)
    {
        if (stateMachine != null)
        {
            var combination = CompanionManager.instance.combinationLibrary.GetCombination(actionList);

            //stateMachine
            CustomEvent.Trigger(this.gameObject, "attempt", combination);
        }
    }

    public void SetRelatedLink(bool state)
    {
        if (offMeshLink == null)
            return;

        offMeshLink.activated = state;

        offMeshLink.GetComponentInChildren<Collider>().enabled = state;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (linkedModal == null && stateMachine == true)
            print("No modal linked with object");

        if (stateMachine != null && linkedModal != null)
        {
            var combination = CompanionManager.instance.combinationLibrary.GetCombination(linkedModal.currentModalActions);

            if (combination == null)
            {
                CustomEvent.Trigger(this.gameObject, "test", combination);
                return;
            }

            //stateMachine
            CustomEvent.Trigger(this.gameObject, "attempt", combination);
        }
    }


    public void SetModal(InteractableModal modal)
    {
        linkedModal = modal;
    }

    public Vector3 OffMeshLinkStartPosition()
    {
        if (offMeshLink != null)
            return offMeshLink.startTransform.position;

        return transform.position;
    }


}
