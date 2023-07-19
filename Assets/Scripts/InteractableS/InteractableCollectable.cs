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

    public void Setup(Action action)
    {
        collectableAction = action;
        actionRenderer.materials = new Material[] { actionRenderer.materials[0], action.actionMaterial };
        actionMesh.mesh = action.actionMesh;
    }

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

        if(CompanionManager.instance.currentInteractable == this)
            Collect();

    }

    public void Collect()
    {
        Action storedAction = collectableAction;

        //Check if companion already has an action
        if (CompanionManager.instance.heldAction == null)
        {
            if (parentSlot == null)
                Destroy(gameObject);
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            ReplaceCollectable(CompanionManager.instance.heldAction);
        }

        CompanionManager.instance.SetHeldAction(storedAction);

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

        collectableAction = action;
        actionRenderer.materials = new Material[] { actionRenderer.materials[0], action.actionMaterial };
        actionMesh.mesh = action.actionMesh;
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
