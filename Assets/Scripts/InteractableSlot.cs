using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableSlot : Interactable
{
    [SerializeField] private Transform slotActionReference;
    [SerializeField] private InteractableCollectable interactable;

    public override void Highlight(bool state)
    {
        //base.Highlight(state);
    }

    public void FillSlot(bool fill, Action action)
    {
        if (interactable.enabled)
            interactable.Collect();

        interactable.collectableAction = action;
        interactable.Setup();
        interactable.enabled = true;
        slotActionReference.localScale = Vector3.one;
        slotActionReference.localPosition = Vector3.zero;
        slotActionReference.gameObject.SetActive(fill);
    }

    private void OnMouseEnter()
    {
        CompanionManager.instance.currentSlot = this;
        if(CompanionManager.instance.selectedInteractable != null)
            transform.DOScale(1.2f, .15f).SetEase(Ease.OutBack);
    }

    private void OnMouseExit()
    {
        CompanionManager.instance.currentSlot = null;

        transform.DOScale(1, .15f).SetEase(Ease.OutBack);
    }
}
