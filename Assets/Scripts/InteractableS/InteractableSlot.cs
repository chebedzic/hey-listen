using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableSlot : Interactable
{
    public ActionType slotType;
    [SerializeField] private Transform slotActionReference;
    public InteractableCollectable insideCollectable;
    private InteractableModal modalParent;

    private void Start()
    {
        modalParent = GetComponentInParent<InteractableModal>();
    }

    public void FillSlot(Action action)
    {
        if (action != null)
        {
            if (action.actionType != slotType)
            {
                modalParent.transform.DOComplete();
                modalParent.transform.DOPunchPosition(modalParent.transform.right / 3, .2f, 20, 1);
                return;
            }

            modalParent.transform.DOComplete();
            modalParent.transform.DOPunchScale(Vector3.one / 6, .2f, 15, 1);

            insideCollectable.OnMouseEnter();
        }

        insideCollectable.Collect();
        insideCollectable.collectableAction = action;

        slotActionReference.localScale = Vector3.one;
        slotActionReference.localPosition = Vector3.zero;
        slotActionReference.DOComplete();
        slotActionReference.DOShakeScale(.2f, .5f, 20, 90, true);
        slotActionReference.gameObject.SetActive(action != null);

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        modalParent.SlotUpdated();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        FillSlot(CompanionManager.instance.heldAction);

    }

    public override void OnMouseEnter()
    {
        if(CompanionManager.instance.heldAction == null && insideCollectable.collectableAction == null)
        {
            modalParent.transform.DOComplete();
            modalParent.transform.DOScale(1.1f, .3f).SetEase(Ease.OutBack);
            return;
        }

        base.OnMouseEnter();
        CompanionManager.instance.currentSlot = this;
        
        insideCollectable.OnMouseEnter();

    }

    public override void OnMouseExit()
    {
        modalParent.transform.DOComplete();
        modalParent.transform.DOScale(1, .3f).SetEase(Ease.OutBack);

        base.OnMouseExit();
        CompanionManager.instance.currentSlot = null;

        if (insideCollectable.collectableAction != null)
        {
            insideCollectable.OnMouseExit();
        }

    }

    public Action SlotAction()
    {
        return insideCollectable.collectableAction;
    }

}
