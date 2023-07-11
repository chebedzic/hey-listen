using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSlot : Interactable
{
    [SerializeField] private Transform slotActionReference;
    [SerializeField] private InteractableCollectable interactable;

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
}
