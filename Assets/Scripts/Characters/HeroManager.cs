using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroManager : MonoBehaviour
{
    public static HeroManager instance;

    [HideInInspector] public UnityEvent<Equipment> OnGetEquipment;

    [HideInInspector] public HeroVisual heroVisual;
    private NavMeshAgent navMeshAgent;

    [Header("States")]
    public bool canMove = true;
    public bool isInteracting = false;
    private bool hasEnteredOffMeshLink = false;

    [Header("Equipment")]
    public Equipment currentEquipment;

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


    public void SetHeroEquipment(Equipment equipment)
    {
        currentEquipment = equipment;

        OnGetEquipment?.Invoke(currentEquipment);
    }

    public bool SetHeroDestination(Vector3 destination)
    {
        if (!canMove)
            return false;

        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(destination, path))
        {
            navMeshAgent.SetPath(path);
            return path.status == NavMeshPathStatus.PathComplete;
        }
        else
            return false;   
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
