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

    public bool ignoreType;

    public MeshFilter phantomMeshFilter;

    private void Awake()
    {
        
    }

    private void Start()
    {
        modalParent = GetComponentInParent<InteractableModal>();
    }

    public void FillSlot(Action action)
    {
        if (action != null)
        {
            if (action.actionType != slotType && !ignoreType)
            {
                if (modalParent != null)
                {
                    modalParent.transform.DOComplete();
                    modalParent.transform.DOPunchPosition(modalParent.transform.right / 3, .5f, 10, 1);
                }
                return;
            }

            if (modalParent != null)
            {
                modalParent.transform.DOComplete();
                modalParent.transform.DOPunchScale(Vector3.one / 6, .2f, 15, 1);
            }

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
        if(modalParent!= null)
            modalParent.SlotUpdated();
    }

    public override void OnMouseDown()
    {
        if (CompanionManager.instance.heldAction == null && insideCollectable.collectableAction == null)
        {
            modalParent.transform.DOComplete();
            modalParent.transform.DOScale(1.1f, .3f).SetEase(Ease.OutBack);
            return;
        }

        base.OnMouseDown();

        if (CompanionManager.instance.heldItem != null)
            return;

        FillSlot(CompanionManager.instance.heldAction);

    }

    public override void OnMouseEnter()
    {
        Action held = CompanionManager.instance.heldAction;
        Action slotAction = insideCollectable.collectableAction;
        CompanionManager.instance.currentSlot = this;

        if (held == null && slotAction == null)
        {
            if (modalParent != null)
            {
                modalParent.transform.DOComplete();
                modalParent.transform.DOScale(1.1f, .3f).SetEase(Ease.OutBack);
            }
            return;
        }

        if(held != null)
        {
            if (held.actionType != slotType && !ignoreType)
                return;
        }

        if (phantomMeshFilter != null && held != null)
            phantomMeshFilter.mesh = held.actionMesh;

        base.OnMouseEnter();


        insideCollectable.OnMouseEnter();

    }

    public override void OnMouseExit()
    {
        if (modalParent != null)
        {
            modalParent.transform.DOComplete();
            modalParent.transform.DOScale(1, .3f).SetEase(Ease.OutBack);
        }

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
