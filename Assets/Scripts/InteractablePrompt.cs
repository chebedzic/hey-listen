using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePrompt : Interactable
{
    [SerializeField] private Transform modalReference;
    [SerializeField] private List<Action> modalActions;
    private ModalScript modalScript;
    private int slotAmount;

    public enum PrompType { confirm, cancel}
    public PrompType prompt;

    private void Start()
    {
        modalScript = GetComponentInParent<ModalScript>();
        slotAmount = transform.parent.GetComponentsInChildren<InteractableSlot>().Length;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (prompt == PrompType.cancel)
            return;

        modalActions = new List<Action>();

        foreach (InteractableSlot slot in transform.parent.GetComponentsInChildren<InteractableSlot>())
        {
            if (slot.interactable.enabled) modalActions.Add(slot.SlotAction());
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
            CompanionManager.instance.SetEditMode(false);
            modalScript.AttemptSolution(modalActions);
        }
    }
}
