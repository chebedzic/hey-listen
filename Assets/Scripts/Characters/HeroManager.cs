using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroManager : MonoBehaviour
{
    public static HeroManager instance;
    [HideInInspector] public HeroVisual heroVisual;

    private NavMeshAgent navMeshAgent;
    public bool canMove = true;
    public bool isInteracting = false;
    private bool hasEnteredOffMeshLink = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        heroVisual = GetComponentInChildren<HeroVisual>();
        CompanionManager.instance.OnEditorMode.AddListener(SetMovementAvailability);
    }

    void Update()
    {
        if (!navMeshAgent.isOnNavMesh)
            return;


        if (navMeshAgent.isOnOffMeshLink && !hasEnteredOffMeshLink)
        {
            SetHeroDestination(navMeshAgent.currentOffMeshLinkData.endPos);
            hasEnteredOffMeshLink = true;
        }

        if(navMeshAgent.remainingDistance < .1f && !navMeshAgent.isOnOffMeshLink)
        {
            if (hasEnteredOffMeshLink)
                hasEnteredOffMeshLink = false;
        }

    }


    void SetMovementAvailability(bool state)
    {
        canMove = !state;
    }

    public void SetHeroDestination(Vector3 destination)
    {
        if (!canMove)
            return;

        navMeshAgent.SetDestination(destination);
    }

    public float AgentRemainingDistance()
    {
        return navMeshAgent.remainingDistance;
    }

    public bool AgentIsStopped()
    {
        return navMeshAgent.velocity.magnitude <= 0;
    }

    public float GetHeroVelocity()
    {
        return navMeshAgent.velocity.magnitude;
    }

    public bool IsAgentCrossingLink()
    {
        return navMeshAgent.isOnOffMeshLink;
    }

    public float GetHeroSpeed()
    {
        return navMeshAgent.speed;
    }
}
