using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroManager : MonoBehaviour
{
    public static HeroManager instance;

    private NavMeshAgent navMeshAgent;
    bool canMove = true;
    private bool hasEnteredOffMeshLink = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        CompanionManager.instance.OnEditorMode.AddListener(SetMovementAvailability);
    }

    void Update()
    {
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
        //TODO change this to state machine, where we can know when the entire game is in edit mode

        if (!canMove)
            return;

        navMeshAgent.SetDestination(destination);
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
