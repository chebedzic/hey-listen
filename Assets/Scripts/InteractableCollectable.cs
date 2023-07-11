using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableCollectable : Interactable
{
    [SerializeField] private Action collectableAction;

    public override void ClickHandler()
    {
        base.ClickHandler();
        enabled = false;

        Vector2 topOfScreenPos = new Vector2(Screen.width / 2, Screen.height);
        Vector3 pos = Camera.main.ScreenToWorldPoint(topOfScreenPos);
        pos += (Camera.main.transform.forward * 6);
        pos += (Camera.main.transform.up * 3);
        transform.DOJump(pos, 1,1,.4f).OnComplete(()=>Collect());
        transform.DOScale(0, .4f);
    }

    void Collect()
    {
        ActionsManager.instance.TryCollectAction(collectableAction);
        Destroy(gameObject);

    }
}
