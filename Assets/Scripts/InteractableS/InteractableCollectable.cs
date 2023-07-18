using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class InteractableCollectable : Interactable
{
    [HideInInspector] public UnityEvent OnCollect;

    public Action collectableAction;
    public InteractableSlot parentSlot;

    [SerializeField] private Renderer actionRenderer;
    [SerializeField] private MeshFilter actionMesh;

    public override void Awake()
    {
        base.Awake();
        actionRenderer = GetComponentInChildren<Renderer>();
        actionMesh = GetComponentInChildren<MeshFilter>();

        if (GetComponentInParent<InteractableSlot>() != null)
            parentSlot = GetComponentInParent<InteractableSlot>();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        Collect();

    }

    public void Collect()
    {
        Action storedAction = collectableAction;

        //Check if companion already has an action
        if (CompanionManager.instance.holdedAction == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ReplaceCollectable(CompanionManager.instance.holdedAction);
        }

        CompanionManager.instance.SetHoldedAction(storedAction);

        OnCollect.Invoke();
    }


    public void ReplaceCollectable(Action action)
    {
        if(action == null)
        {
            collectableAction = null;
            //gameObject
            return;
        }

        actionRenderer.materials = new Material[] { actionRenderer.materials[0], action.actionMaterial };
        actionMesh.mesh = action.actionMesh;
        collectableAction = action;
    }


    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }


    public override void OnMouseExit()
    {
        base.OnMouseExit();

    }


}
