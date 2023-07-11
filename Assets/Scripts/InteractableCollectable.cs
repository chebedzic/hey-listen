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

    public override void ClickHandler()
    {
        if (!enabled)
            return;

        enabled = false;

        Vector2 topOfScreenPos = new Vector2(Screen.width / 2, Screen.height);
        Vector3 pos = Camera.main.ScreenToWorldPoint(topOfScreenPos);
        pos += (Camera.main.transform.forward * 6);
        pos += (Camera.main.transform.up * 3);
        transform.DOJump(pos, 1,1,.4f).OnComplete(()=>Collect());
        transform.DOScale(.3f, .4f);
    }

    public void Collect()
    {
        if (GetComponentInParent<InteractableSlot>() != null)
        {

        }

        ActionsManager.instance.TryCollectAction(collectableAction);
        gameObject.SetActive(false);

    }

    public void Setup()
    {
        actionRenderer.materials = new Material[] { actionRenderer.materials[0], collectableAction.actionMaterial };
    }
}
