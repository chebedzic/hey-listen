using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Unity.VisualScripting;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    public bool enabled = true;

    public UnityEvent OnClick;

    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;

    [Header("Puzzle")]
    private StateMachine stateMachine;
    [SerializeField] private FlowGraph flow;

    private ModalScript linkedModal;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        interactableRenderers = GetComponentsInChildren<Renderer>();
    }

    public void SetModal(ModalScript modal)
    {
        linkedModal = modal;
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

        if (linkedModal == null)
            print("No modal linked with object");

        if (stateMachine != null && linkedModal != null)
        {
            var combination = CompanionManager.instance.combinationLibrary.GetCombination(linkedModal.currentModalActions);

            if (combination == null)
            {
                CustomEvent.Trigger(this.gameObject, "test", combination);
                return;
            }

            //stateMachine
            CustomEvent.Trigger(this.gameObject, "attempt", combination);
        }
    }

    public void TryPuzzle(List<Action> actionList, ModalScript modal)
    {
        if (stateMachine != null)
        {
            var combination = CompanionManager.instance.combinationLibrary.GetCombination(actionList);
 
            //stateMachine
            CustomEvent.Trigger(this.gameObject, "attempt", combination);
        }
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
