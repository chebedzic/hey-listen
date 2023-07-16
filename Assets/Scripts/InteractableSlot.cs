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
    }

    public override void Highlight(bool state)
    {
        //base.Highlight(state);
        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
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
        base.OnMouseEnter();
        CompanionManager.instance.currentSlot = this;
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        CompanionManager.instance.currentSlot = null;
    }

    public Action SlotAction()
    {
        return interactableCollectable.collectableAction;
    }

}
