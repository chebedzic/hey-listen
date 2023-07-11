using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePrompt : Interactable
{
    [SerializeField] private Transform modalReference;
    private List<Action> modalActions;

    private void Start()
    {
 
    }

    public override void OnMouseDown()
    {
        //TODO
        base.OnMouseDown();

        modalActions = new List<Action>();

        for (int i = 0; i < modalReference.childCount; i++)
        {
            int index = i;
            InteractableSlot slot = modalReference.GetChild(index).GetComponentInChildren<InteractableSlot>();
            if(slot.interactable.enabled) modalActions.Add(slot.SlotAction());
        }

        string names = "Confirmed choice: ";

        foreach (Action a in modalActions)
        {
            names += "<color=#"+ColorUtility.ToHtmlStringRGB(a.actionColor)+">"+ a.actionName + "</color>  +  ";
        }

        if(modalActions.Count>0)
            print(names);
    }
}
