using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class InteractablePuzzle : Interactable
{

    [Header("Puzzle")]
    [SerializeField] private OffMeshLink offMeshLink;
    private StateMachine stateMachine;
    private InteractableModal linkedModal;
    private Animator animator;

    [Header("Settings")]
    [SerializeField] private float heroDistance = 1.0f;
    [HideInInspector] public HeroManager heroManager;
    [HideInInspector] public HeroVisual heroVisual;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        stateMachine = GetComponent<StateMachine>();
        heroManager = HeroManager.instance;
        heroVisual = heroManager.heroVisual;
    }

    public void TryPuzzle(List<Action> actionList, InteractableModal modal)
    {
        if (stateMachine != null && !heroManager.isInteracting)
        {
            heroManager.isInteracting = true;
            ActionCombination combination = CompanionManager.instance.combinationLibrary.GetCombination(actionList);
            StartCoroutine(BringHero(combination));

        }
    }

    public void SetHeroInteraction(bool isInteracting)
    {
        if(heroManager!= null)
            heroManager.isInteracting = isInteracting;
    }

    IEnumerator BringHero(ActionCombination combination)
    {
        HeroManager.instance.SetHeroDestination(transform.position + (transform.forward * heroDistance));
        yield return new WaitForSeconds(.2f);
        yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
        HeroManager.instance.SetHeroDestination(HeroManager.instance.transform.position);
        HeroManager.instance.transform.DOLookAt(transform.position, .3f, AxisConstraint.Y);
        CustomEvent.Trigger(this.gameObject, "TryInteraction", combination);

    }

    public void SetRelatedLink(bool state)
    {
        if (offMeshLink == null)
            return;

        offMeshLink.activated = state;

        offMeshLink.GetComponentInChildren<Collider>().enabled = state;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (linkedModal == null && stateMachine == true)
            print("No modal linked with object");

        if (stateMachine != null && linkedModal != null)
        {
            TryPuzzle(linkedModal.currentModalActions, linkedModal);
        }
    }

    public void TriggerAnimator(string trigger)
    {
        if(animator)
            animator.SetTrigger(trigger);
    }


    public void SetModal(InteractableModal modal)
    {
        linkedModal = modal;
    }

    public void ShakeCamera(Vector3 velocity, float duration)
    {
        CinemachineImpulseSource impulseSource = Camera.main.GetComponent<CinemachineImpulseSource>();
        impulseSource.m_DefaultVelocity = velocity;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
        impulseSource.GenerateImpulse();
    }

    public void MoveHeroTween(Vector3 finalPos, float duration)
    {
        HeroManager.instance.transform.DOMove(finalPos, duration);
    }

    public void JumpHeroTween(Vector3 finalPos, float jumpPower, float duration)
    {
        HeroManager.instance.GetComponent<NavMeshAgent>().updateRotation = false;
        HeroManager.instance.transform.DOJump(finalPos, jumpPower,1, duration).OnComplete(()=> HeroManager.instance.GetComponent<NavMeshAgent>().updateRotation = true);
        HeroManager.instance.SetHeroDestination(finalPos);
    }

    public Vector3 OffMeshLinkStartPosition()
    {
        if (offMeshLink != null)
            return offMeshLink.startTransform.position;

        return transform.position;
    }


}
