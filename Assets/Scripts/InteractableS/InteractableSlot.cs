using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableSlot : Interactable
{
    public ActionType slotType;
    [SerializeField] private Transform slotActionReference;
    public InteractableCollectable interactableCollectable;
    private InteractableModal modalParent;

    private void Start()
    {
        modalParent = GetComponentInParent<InteractableModal>();
        interactableCollider.enabled = false;
    }

    public void FillSlot(bool fill, Action action)
    {
        if (interactableCollectable.gameObject.activeSelf)
            interactableCollectable.Collect();

        interactableCollectable.collectableAction = action;
        interactableCollectable.Setup();
        interactableCollectable.interactable = true;
        slotActionReference.localScale = Vector3.one;
        slotActionReference.localPosition = Vector3.zero;
        slotActionReference.DOComplete();
        slotActionReference.DOShakeScale(.2f, .5f, 20, 90, true);
        slotActionReference.gameObject.SetActive(fill);

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        modalParent.SlotUpdated();
    }

    public override void OnMouseEnter()
    {
        if (CompanionManager.instance.focusedModal == null)
            return;

        base.OnMouseEnter();
        CompanionManager.instance.currentSlot = this;
    }

    public override void OnMouseExit()
    {
        if (CompanionManager.instance.focusedModal == null)
            return;

        base.OnMouseExit();
        CompanionManager.instance.currentSlot = null;

    }

    public Action SlotAction()
    {
        return interactableCollectable.collectableAction;
    }

}
