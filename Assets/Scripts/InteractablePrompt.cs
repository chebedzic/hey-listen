using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePrompt : Interactable
{
    [SerializeField] private Transform modalReference;
    [SerializeField] private List<Action> modalActions;
    private InteractableModal modalScript;
    private int slotAmount;

    public enum PrompType { confirm, cancel}
    public PrompType prompt;

    private void Start()
    {
        modalScript = GetComponentInParent<InteractableModal>();
        slotAmount = transform.parent.GetComponentsInChildren<InteractableSlot>().Length;
    }

    public override void OnMouseEnter()
    {
        if (CompanionManager.instance.focusedModal != modalScript)
            return; 

        base.OnMouseEnter();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (CompanionManager.instance.focusedModal != modalScript)
            return;

        if (prompt == PrompType.cancel)
        {
            modalScript.SetModalForEditMode(false);
            return;
        }

        modalActions = new List<Action>();

        foreach (InteractableSlot slot in transform.parent.GetComponentsInChildren<InteractableSlot>())
        {
            if (slot.interactableCollectable.interactable) modalActions.Add(slot.SlotAction());
        }

        string names = "Confirmed choice: ";

        for (int i = 0; i < modalActions.Count; i++)
        {
            Action a = modalActions[i];
            names += "<b><color=#" + ColorUtility.ToHtmlStringRGB(a.actionColor) + ">" + a.actionName + "</color></b>"+ (i < modalActions.Count-1 ? "  +  " : string.Empty);

        }
        if (modalActions.Count >= slotAmount)
        {
            print(names);
            modalScript.SetModalForEditMode(false);
            modalScript.AttemptSolution(modalActions);
        }
    }
}
