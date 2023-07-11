using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeroManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    bool canMove = true;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        CompanionManager.instance.OnMouseClick.AddListener(SetHeroDestination);
        CompanionManager.instance.OnEditorMode.AddListener(SetMovementAvailability);
    }

    void Update()
    {
        
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

    public float GetHeroSpeed()
    {
        return navMeshAgent.speed;
    }
}
