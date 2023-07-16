using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.Events;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    public UnityEvent OnClick;

    [Header("States")]
    public bool interactable = true;
    public bool inFocus = false;

    [HideInInspector] public Collider interactableCollider;
    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;

    public virtual void Awake()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactableCollider = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        if(transform.childCount> 0)
            if(transform.GetChild(0)!=null)
                transform.GetChild(0).DOComplete();
        transform.DOComplete();
    }

    public virtual void OnMouseDown()
    {
        //Base
        OnClick?.Invoke();
    }
   
    public virtual void OnMouseEnter()
    {

        CompanionManager.instance.currentInteractable = this;
        Highlight(true);

        CursorHandler.instance.HoverInteractable(true, CursorType.hover);
    }

    public virtual void OnMouseExit()
    {

        CompanionManager.instance.currentInteractable = null;
        Highlight(false);

        CursorHandler.instance.HoverInteractable(false, CursorType.hover);

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
                if (mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
    }
    public void SetColliderState(bool enabled)
    {
        interactableCollider.enabled = enabled;
    }

    public virtual void OnMouseDrag() { }

    public virtual void OnMouseUp() { }

    public Animator GetHeroAnimator() { return HeroManager.instance.GetComponentInChildren<Animator>(); }

    public NavMeshAgent GetHeroAgent() { return HeroManager.instance.GetComponent<NavMeshAgent>(); }

}
