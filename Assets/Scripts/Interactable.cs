using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    public UnityEvent OnClick;

    public bool enabled = true;
    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;

    private void Awake()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
    }

    public virtual void Highlight(bool state)
    {

        if (state && transform.childCount > 0)
        {
            transform.GetChild(0).DOComplete();
            transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 20, true);
        }

        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if(mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
    }


    private void OnDestroy()
    {
        if(transform.GetChild(0)!=null)
            transform.GetChild(0).DOComplete();
        transform.DOComplete();
    }

    public virtual void OnMouseDown()
    {
        OnClick.Invoke();
    }

    public virtual void OnMouseEnter()
    {
        CompanionManager.instance.currentInteractable = this;
        Highlight(true);

        CursorHandler.instance.HoverInteractable(true);
    }

    public virtual void OnMouseExit()
    {
        CompanionManager.instance.currentInteractable = null;
        Highlight(false);

        CursorHandler.instance.HoverInteractable(false);

    }

    public virtual void OnMouseDrag() { }

    public virtual void OnMouseUp() { }

}
