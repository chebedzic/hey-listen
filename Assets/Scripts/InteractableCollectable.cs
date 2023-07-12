using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableCollectable : Interactable
{
    [SerializeField] public Action collectableAction;

    [SerializeField] private Renderer actionRenderer;

    private void Start()
    {
        actionRenderer = GetComponentInChildren<Renderer>();
    }

    public override void OnMouseDown()
    {
        if (!enabled)
            return;

        enabled = false;

        Vector2 topOfScreenPos = new Vector2(Screen.width / 2, Screen.height);
        Vector3 pos = Camera.main.ScreenToWorldPoint(topOfScreenPos);
        pos += (Camera.main.transform.forward * 6);
        pos += (Camera.main.transform.up * 2.82f);
        Collect();
        //transform.DOJump(pos, .5f,1,.4f).OnComplete(()=>Collect());
        //transform.GetChild(0).DOComplete();
        //transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 90, true);
        //transform.DOScale(.3f, .4f);
    }

    public void Collect()
    {
        ActionsManager.instance.TryCollectAction(collectableAction);
        gameObject.SetActive(false);
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
