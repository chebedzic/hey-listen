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

    [HideInInspector] public Collider[] interactableColliders;
    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;
    [SerializeField] private bool disableWhenHeroIsInteracting = true;

    private int originalLayer;

    public virtual void Update()
    {
        if (!disableWhenHeroIsInteracting)
            return;

        gameObject.layer = HeroManager.instance.isInteracting ? 2 : originalLayer;
    }

    public virtual void Awake()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactableColliders = GetComponentsInChildren<Collider>();

        originalLayer = gameObject.layer;
    }

    public virtual void Start()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactableColliders = GetComponentsInChildren<Collider>();
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
        //if(HeroManager.instance.isLookingForBridge)
        //    return;
            
        CompanionManager.instance.currentInteractable = this;

        CursorHandler.instance.HoverInteractable(true, CursorType.hover);

        //Event
        OnPointerEnter?.Invoke(true);
    }

    public virtual void OnMouseExit()
    {

        CompanionManager.instance.currentInteractable = null;

        CursorHandler.instance.HoverInteractable(false, CursorType.hover);

        OnPointerEnter?.Invoke(false);

    }

    public void SetColliderState(bool enabled)
    {
        if(interactableColliders.Length <= 0) return;

        foreach (Collider coll in interactableColliders)
        {
            if(coll != null)
            coll.enabled = enabled;
        }
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
