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
    [HideInInspector] public UnityEvent<bool> OnEquipmentFire;

    [HideInInspector] public HeroVisual heroVisual;
    private NavMeshAgent navMeshAgent;

    [Header("States")]
    public bool isInteracting = false;
    public bool isLookingForBridge = false;
    private bool hasEnteredOffMeshLink = false;

    [Header("Equipment")]
    public Equipment currentEquipment;
    public bool equipmentIsOnFire;
    public float fireInterval = 4;
    Coroutine equipmentFireCoroutine;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        heroVisual = GetComponentInChildren<HeroVisual>();
    }

    void Update()
    {
        if (!navMeshAgent.isOnNavMesh)
            return;


        if (navMeshAgent.isOnOffMeshLink && !hasEnteredOffMeshLink)
        {
            SetHeroDestination(navMeshAgent.currentOffMeshLinkData.endPos, false);
            hasEnteredOffMeshLink = true;
        }

        if (navMeshAgent.remainingDistance < .1f && !navMeshAgent.isOnOffMeshLink)
        {
            if (hasEnteredOffMeshLink)
                hasEnteredOffMeshLink = false;  
        }

    }

    public void SetHeroEquipment(Equipment equipment)
    {
        currentEquipment = equipment;

        OnGetEquipment?.Invoke(currentEquipment);
    }

    public void SetEquipmentState(bool fire)
    {
        equipmentIsOnFire = fire;
        OnEquipmentFire?.Invoke(equipmentIsOnFire);

        if (fire)
        {
            if (equipmentFireCoroutine != null) StopCoroutine(equipmentFireCoroutine);
            equipmentFireCoroutine = StartCoroutine(FireCoroutine());

            IEnumerator FireCoroutine()
            {
                yield return new WaitForSeconds(fireInterval);
                SetEquipmentState(false);
            }
        }

    }
    public void UpdateNavAgentPosition(bool update)
    {
        if (update)
        {
            navMeshAgent.SetDestination(transform.position);
        }
        navMeshAgent.updatePosition = update;
    }

    public bool SetHeroDestination(Vector3 destination, bool calculatePath = true)
    {

        if (calculatePath)
        {
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(destination, path))
            {
                navMeshAgent.SetPath(path);
                return path.status == NavMeshPathStatus.PathComplete;
            }
            return false;
        }
        else
        {
            return navMeshAgent.SetDestination(destination);
        }
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
