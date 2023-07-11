using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableSlot : Interactable
{

    public ActionType slotType;
    [SerializeField] private Transform slotActionReference;
    public InteractableCollectable interactable;

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
        slotActionReference.DOComplete();
        slotActionReference.DOShakeScale(.2f, .5f, 20, 90, true);
        slotActionReference.gameObject.SetActive(fill);
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        CompanionManager.instance.currentSlot = this;
        if (CompanionManager.instance.selectedInteractable != null)
            transform.DOScale(1.2f, .15f).SetEase(Ease.OutBack);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        CompanionManager.instance.currentSlot = null;
        transform.DOScale(1, .15f).SetEase(Ease.OutBack);
    }

    public Action SlotAction()
    {
        return interactable.collectableAction;
    }

}
