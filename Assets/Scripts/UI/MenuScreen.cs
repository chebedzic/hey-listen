using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class MenuScreen : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected CanvasGroup canvasGroup;
    public bool isActive;
    public void SetActive(bool active)
    {
        if(isActive == active) 
            return;
        isActive = active;

        if (active)
            Activate();
        else
            Deactivate();
    }
    protected virtual void Activate()
    {
        canvas.enabled = true;
        canvasGroup.DOFade(1, 0.3f).From(0);

    }
    protected virtual void Deactivate()
    {
      canvasGroup.DOFade(0, 0.3f).OnComplete(()=>canvas.enabled = false);
    }
}
