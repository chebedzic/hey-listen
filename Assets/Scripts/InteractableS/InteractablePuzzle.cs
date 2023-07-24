using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class InteractablePuzzle : Interactable
{

    public UnityEvent OnFirstInteraction;

    [Header("Puzzle")]
    public OffMeshLink offMeshLink;
    private StateMachine stateMachine;
    private InteractableModal linkedModal;
    private Animator[] animators;
    private ParticleSystem[] particleSystems;

    [Header("Settings")]
    [SerializeField] private bool heroWalksToInteraction = true;
    [SerializeField] private bool positionHeroInFront = false;
    [SerializeField] private float heroDistance = 1.0f;
    [HideInInspector] public HeroManager heroManager;
    [HideInInspector] public HeroVisual heroVisual;

    public bool modalRevealed;

    public override void Start()
    {
        base.Start();

        StartCoroutine(StartSequence());

        IEnumerator StartSequence()
        {
            yield return new WaitForEndOfFrame();

            if (linkedModal)
                linkedModal.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public override void Awake()
    {
        base.Awake();
        animators = GetComponentsInChildren<Animator>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        stateMachine = GetComponent<StateMachine>();
        heroManager = HeroManager.instance;
        heroVisual = heroManager.heroVisual;
    }

    public void TryPuzzle(List<Action> actionList, InteractableModal modal)
    {
        if(!gameObject.activeSelf) return;

        if (stateMachine != null && !heroManager.isInteracting && GetComponent<Collider>().enabled)
        {
            if (stateMachine.graph == null)
                return;

            heroManager.isInteracting = true;
            ActionCombination combination = CompanionManager.instance.combinationLibrary.GetCombination(actionList);

            if(heroWalksToInteraction)
                StartCoroutine(BringHero(combination));
            else
            {
                CustomEvent.Trigger(this.gameObject, "TryInteraction", combination);
            }
        }
    }

    public void SetHeroInteraction(bool isInteracting)
    {
        if(heroManager != null)
            heroManager.isInteracting = isInteracting;
    }

    public void InvokeFirstInteraction()
    {
        OnFirstInteraction?.Invoke();
    }

    public void SetHeroZDestination(float z)
    {
        var hero = GetHeroAgent();
        hero.SetDestination(hero.transform.position + (Vector3.forward * z));
    }

    IEnumerator BringHero(ActionCombination combination)
    {
        Vector3 placementPosition = positionHeroInFront ? transform.position + (transform.forward * heroDistance) : transform.position + ((heroManager.transform.position - transform.position) * heroDistance);
        placementPosition = offMeshLink != null && transform.CompareTag("Door") ? offMeshLink.startTransform.position : placementPosition;
        if (!(Vector3.Distance(heroManager.transform.position, placementPosition) <= heroDistance))
            if (!HeroManager.instance.SetHeroDestination(placementPosition)){
                //can't reach interaction
                heroManager.isInteracting = false;
                yield break;
            }
        yield return new WaitForSeconds(.2f);
        yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
        HeroManager.instance.SetHeroDestination(HeroManager.instance.transform.position);
        HeroManager.instance.transform.DOLookAt(transform.position, .3f, AxisConstraint.Y);
        //combination = CompanionManager.instance.combinationLibrary.GetCombination(linkedModal.actionList);
        CustomEvent.Trigger(this.gameObject, "TryInteraction", combination);
    }

    public void SetRelatedLink(bool state, bool tryBridge)
    {
        if (offMeshLink == null)
            return;

        offMeshLink.activated = state;

        if (!state)
            return;

        if (!tryBridge)
            return;

        if (offMeshLink.GetComponentInChildren<RoomBridge>() != null)
            offMeshLink.GetComponentInChildren<RoomBridge>().TryBridge();
    }

    public void RevealModal()
    {
        modalRevealed = true;
        linkedModal.transform.GetChild(0).gameObject.SetActive(true);
        linkedModal.transform.GetChild(0).DOComplete();
        linkedModal.transform.GetChild(0).DOScale(0, .5f).From();

    }


    //A function I made specifically when you are returning from a room connected to a door interactable
    public void BackToRoom(Vector3 finalPos)
    {
        StartCoroutine(LinkedDoor());

        IEnumerator LinkedDoor()
        {
            offMeshLink.activated = true;
            HeroManager.instance.isInteracting = true;
            HeroManager.instance.SetHeroDestination(finalPos);
            yield return new WaitForSeconds(.2f);
            yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
            yield return new WaitUntil(() => !HeroManager.instance.IsAgentCrossingLink());
            yield return new WaitForSeconds(.2f);
            HeroManager.instance.isInteracting = false;
            if (GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().SetTrigger("close");
            offMeshLink.activated = false;


            SetRelatedLink(false, true);
        }
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (linkedModal == null && stateMachine == true)
            print("No modal linked with object");

        if (stateMachine != null && linkedModal != null)
        {
            TryPuzzle(linkedModal.actionList, linkedModal);
        }
    }

    //Only reference by distance trigger that I made for testing
    public void TriggerPuzzle()
    {

        if (linkedModal == null && stateMachine == true)
            print("No modal linked with object");

        TryPuzzle(linkedModal.actionList, linkedModal);
    }

    public void TriggerAnimator(string trigger, int animatorIndex = -1)
    {
        if (animators.Length <= 0)
            return;

        if (animatorIndex != -1)
        {
            animators[animatorIndex].SetTrigger(trigger);
            return;
        }

        foreach (Animator animator in animators)
        {
            animator.SetTrigger(trigger);
        }
    }

    public void TriggerParticles(int particlesIndex = -1)
    {
        if (particleSystems.Length <= 0)
            return;

        if (particlesIndex != -1)
        {
            particleSystems[particlesIndex].Play();
            return;
        }

        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Play();
        }
    }


    public void SetModal(InteractableModal modal)
    {
        linkedModal = modal;
    }

    public Vector3 OffMeshLinkStartPosition()
    {
        if (offMeshLink != null)
            return offMeshLink.startTransform.position;

        return transform.position;
    }

    public void SetHeroDestinationAtMeshLinkStartPos()
    {
        if (offMeshLink == null)
            return;

        heroManager.GetComponent<NavMeshAgent>().SetDestination(GetClosestOfflinkPlacement());
    }

    public void SetHeroDestinationAtMeshLinkEndPos()
    {
        if (offMeshLink == null)
            return;

        heroManager.GetComponent<NavMeshAgent>().SetDestination(GetFurthestOfflinkPlacement());
    }


    public Vector3 GetFurthestOfflinkPlacement()
    {
        Vector3[] offLinkPoints = new Vector3[] { offMeshLink.startTransform.position, offMeshLink.endTransform.position };
        return offLinkPoints.OrderBy(p => Vector3.Distance(p, heroManager.transform.position)).Last();
    }

    public Vector3 GetClosestOfflinkPlacement()
    {
        Vector3[] offLinkPoints = new Vector3[] { offMeshLink.startTransform.position, offMeshLink.endTransform.position };
        return offLinkPoints.OrderBy(p => Vector3.Distance(p, heroManager.transform.position)).First();
    }

    //Animations

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

    public void BlinkEmission()
    {
        foreach (Renderer renderer in interactableRenderers)
        {
            if (!renderer.material.HasFloat("_FresnelAmount"))
                break;

            renderer.material.DOComplete();
            renderer.material.SetColor("_FresnelColor", Color.red);
            renderer.material.DOFloat(1, "_FresnelAmount", .1f).OnComplete(() => renderer.material.DOFloat(0, "_FresnelAmount", .2f));
        }
    }

}
