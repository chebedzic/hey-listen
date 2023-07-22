using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class InteractableModal : Interactable
{
    public Transform companionPositionReference;

    [Header("Puzzle")]
    [SerializeField] private InteractablePuzzle linkedPuzzle;
    [SerializeField] private OffMeshLink offMeshLink;

    private InteractableSlot[] slots;

    public List<Action> actionList;

    void Start()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
        //Get inside slots
        slots = GetComponentsInChildren<InteractableSlot>();

        //Check if puzzle is valid
        if (linkedPuzzle)
            linkedPuzzle.SetModal(this);

    }


    public override void OnMouseEnter()
    {
    }

    public override void OnMouseExit()
    {
    }

    // Runs whenever an inside slot is updated
    public void SlotUpdated()
    {
        actionList.Clear();

        foreach (InteractableSlot slot in slots)
        {
            if (slot.insideCollectable.collectableAction != null)
                actionList.Add(slot.insideCollectable.collectableAction);
        }

        HighlightModal(actionList.Count == slots.Length);

    }
    void HighlightModal(bool highlight)
    {
        var visualHandlers = GetComponentsInChildren<InteractableVisualHandler>();
        foreach (InteractableVisualHandler handler in visualHandlers)
        {
            handler.forceHighlight = highlight;
            handler.HoverVisual(highlight);
        }
    }


    public void AttemptSolution(List<Action> actions)
    {
        if (linkedPuzzle == null)
        {
            Debug.LogWarning("No puzzle is linked to modal");
            return;
        }

        linkedPuzzle.TryPuzzle(actions, this);
    }

}
