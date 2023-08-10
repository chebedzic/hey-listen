using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

public class InteractableCollectable : Interactable
{
    [HideInInspector] public UnityEvent OnCollect;

    public Action collectableAction;
    public Item collectableItem;
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

        if(collectableItem != null)
        {
            Hold();
            return;
        }

        if(CompanionManager.instance.currentInteractable == this)
            Collect();

    }

    public void Collect()
    {
        if (CompanionManager.instance.heldItem != null)
            return;

        Action storedAction = collectableAction;

        //Check if companion already has an action
        if (CompanionManager.instance.heldAction == null)
        {
            if (parentSlot == null)
            {
                CompanionManager.instance.currentCollectable = true;
                CompanionManager.instance.CollectableSafetyCooldown();
                Destroy(gameObject);
            }
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

    void Hold()
    {
        CompanionManager.instance.SetHeldItem(collectableItem);
        gameObject.SetActive(false);
    }

    public void SetActionNull()
    {
        collectableAction = null;
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
