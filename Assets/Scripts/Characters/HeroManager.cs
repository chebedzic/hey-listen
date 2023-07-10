using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        CompanionManager.instance.OnMouseClick.AddListener(SetHeroDestination);
    }

    void Update()
    {
        
    }

    public void SetHeroDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    public float GetHeroVelocity()
    {
        return navMeshAgent.velocity.magnitude;
    }

    public float GetHeroSpeed()
    {
        return navMeshAgent.speed;
    }
}
