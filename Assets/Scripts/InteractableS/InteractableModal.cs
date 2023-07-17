using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class InteractableModal : Interactable
{
    [Header("Puzzle")]
    [SerializeField] private InteractablePuzzle linkedPuzzle;
    [SerializeField] private OffMeshLink offMeshLink;

    [Header("Actions")]
    public List<Action> currentModalActions;

    private Collider collider;
    private Vector3 originalPosition;
    private InteractableSlot[] slots;

    [Header("External References")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject cancelButton;

    bool isReady;

    void Start()
    {
        //Get inside slots
        slots = GetComponentsInChildren<InteractableSlot>();
        collider = GetComponent<Collider>();

        //Deactivate the play buttons
        playButton.SetActive(false);
        cancelButton.SetActive(false);

        //Check if puzzle is valid
        if (linkedPuzzle)
            linkedPuzzle.SetModal(this);

        //Remember modal position
        originalPosition = transform.position;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (!CompanionManager.instance.isInEditorMode && CompanionManager.instance.focusedModal == null)
            SetModalForEditMode(true);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();

        CompanionManager.instance.currentModal = null;
        transform.GetChild(0).DOScale(1, .2f);
        CursorHandler.instance.HoverInteractable(false, CursorType.hover);
    }

    // Runs whenever an inside slot is updated
    public void SlotUpdated()
    {
        currentModalActions.Clear();
        int updated = 0;

        foreach (InteractableSlot slot in slots)
        {
            updated += slot.interactableCollectable.gameObject.activeSelf ? 1 : 0;
            if (slot.interactableCollectable.gameObject.activeSelf)
                currentModalActions.Add(slot.interactableCollectable.collectableAction);
            else
            {
                currentModalActions.Remove(slot.interactableCollectable.collectableAction);
            }
        }

        playButton.SetActive(updated == slots.Length);
        isReady = updated == slots.Length;
    }

    public void SetModalForEditMode(bool state)
    {
        collider.enabled = !state;
        CompanionManager.instance.SetEditMode(state, this);

        // Get the main camera
        Camera mainCamera = Camera.main;
        Vector3 modalPos = mainCamera.transform.GetChild(0).position;

        transform.DOMove(state ? new Vector3(modalPos.x, modalPos.y, modalPos.z) : originalPosition, .25f).SetEase(state ? Ease.OutBack : Ease.OutSine);

        cancelButton.SetActive(state);
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
