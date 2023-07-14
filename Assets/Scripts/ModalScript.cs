using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ModalScript : MonoBehaviour
{

    [SerializeField] private Interactable linkedInteractable;
    private Collider collider;
    private Vector3 originalPosition;
    private InteractableSlot[] slots;
    [SerializeField] GameObject playButton;
    public List<Action> currentModalActions;

    private void Start()
    {
        slots = GetComponentsInChildren<InteractableSlot>();
        originalPosition = transform.position;
        collider = GetComponent<Collider>();
        CompanionManager.instance.OnEditorMode.AddListener(OnEditMode);
        playButton.SetActive(false);
        linkedInteractable.SetModal(this);
    }

    public void SlotUpdated()
    {
        currentModalActions.Clear();
        int updated = 0;

        foreach (InteractableSlot slot in slots)
        {
            updated += slot.interactable.enabled ? 1 : 0;
            if(slot.interactable.enabled)
            currentModalActions.Add(slot.interactable.collectableAction);
        }

        playButton.SetActive(updated == slots.Length);


    }

    public void AttemptSolution(List<Action> actions)
    {
        linkedInteractable.TryPuzzle(actions, this);
    }

    private void OnMouseEnter()
    {
        CompanionManager.instance.currentModal = this;
        transform.GetChild(0).DOScale(1.1f, .2f).SetEase(Ease.OutBack);
        CursorHandler.instance.HoverInteractable(true);
    }

    private void OnMouseDown()
    {
        if (!CompanionManager.instance.isInEditorMode)
        {
            CompanionManager.instance.SetEditMode(true);
            collider.enabled = false;

        }
    }

    void OnEditMode(bool state)
    {
        collider.enabled = !state;

        // Get the main camera
        Camera mainCamera = Camera.main;

        // Calculate the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

        // Convert the screen point to world space
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        // Calculate the distance between the camera and the object on the Z axis
        float distanceFromCamera = Mathf.Abs(worldCenter.z - mainCamera.transform.position.z);

        // Calculate the desired Z position based on the camera's field of view
        float desiredZPosition = Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * distanceFromCamera;
        
        transform.DOMove(state ? new Vector3(worldCenter.x, 3, desiredZPosition - 3) : originalPosition, .25f).SetEase(Ease.OutSine);

    }

    private void OnMouseExit()
    {
        CompanionManager.instance.currentModal = null;
        transform.GetChild(0).DOScale(1, .2f);
        CursorHandler.instance.HoverInteractable(false);
    }
}
