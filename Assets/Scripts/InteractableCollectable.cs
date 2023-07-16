using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableCollectable : Interactable
{
    public bool canCollect = true;

    public Action collectableAction;

    public InteractableSlot parentSlot;

    [SerializeField] private Renderer actionRenderer;

    private void Start()
    {
        actionRenderer = GetComponentInChildren<Renderer>();

        if(GetComponentInParent<InteractableSlot>() != null)
            parentSlot = GetComponentInParent<InteractableSlot>();
    }

    public override void OnMouseDown()
    {
        if (!canCollect)
            return;

        if (parentSlot == null)
            canCollect = false;

        Vector2 topOfScreenPos = new Vector2(Screen.width / 2, Screen.height);
        Vector3 pos = Camera.main.ScreenToWorldPoint(topOfScreenPos);
        pos += (Camera.main.transform.forward * 6);
        pos += (Camera.main.transform.up * 2.82f);

        transform.DOJump(pos, .35f, 1, .3f).OnComplete(() => Collect());
        transform.GetChild(0).DOComplete();
        transform.GetChild(0).DOShakeScale(.4f, 1f, 20, 90, true);
        transform.GetChild(0).DORotate(new Vector3(-360, 0, 0), .4f, RotateMode.LocalAxisAdd);
        transform.DOScale(.3f, .4f);
    }

    public void Collect()
    {
        ActionsManager.instance.TryCollectAction(collectableAction);
        gameObject.SetActive(false);

        if (parentSlot != null)
            parentSlot.UpdateSlot();
    }

    public void Setup()
    {
        actionRenderer.materials = new Material[] { actionRenderer.materials[0], collectableAction.actionMaterial };
    }


    public override void OnMouseEnter()
    {
        base.OnMouseEnter();

        if (GetComponentInParent<InteractableSlot>() != null)
        {
            GetComponentInParent<InteractableSlot>().Highlight(true);
            CompanionManager.instance.currentSlot = GetComponentInParent<InteractableSlot>();
        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();

        if (GetComponentInParent<InteractableSlot>() != null)
        {
            GetComponentInParent<InteractableSlot>().Highlight(false);
            CompanionManager.instance.currentSlot = GetComponentInParent<InteractableSlot>();
        }

        CompanionManager.instance.currentSlot = null;

    }


}
