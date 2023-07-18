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
    [HideInInspector] public UnityEvent<bool> OnPointerEnter;

    [Header("States")]
    public bool interactable = true;
    public bool inFocus = false;

    [HideInInspector] public Collider interactableCollider;
    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;

    public virtual void Awake()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactableCollider = GetComponent<Collider>();
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
        //Event
        OnClick?.Invoke();
    }
   
    public virtual void OnMouseEnter()
    {

        CompanionManager.instance.currentInteractable = this;
        //Highlight(true);

        CursorHandler.instance.HoverInteractable(true, CursorType.hover);

        //Event
        OnPointerEnter?.Invoke(true);
    }

    public virtual void OnMouseExit()
    {

        CompanionManager.instance.currentInteractable = null;
        //Highlight(false);

        CursorHandler.instance.HoverInteractable(false, CursorType.hover);

        OnPointerEnter?.Invoke(false);

    }

    public void SetColliderState(bool enabled)
    {
        interactableCollider.enabled = enabled;
    }

    private void OnDisable()
    {
        if(CompanionManager.instance.currentInteractable == this)
            OnMouseExit();
    }

    public virtual void OnMouseDrag() { }

    public virtual void OnMouseUp() { }

    public Animator GetHeroAnimator() { return HeroManager.instance.GetComponentInChildren<Animator>(); }

    public NavMeshAgent GetHeroAgent() { return HeroManager.instance.GetComponent<NavMeshAgent>(); }

}
