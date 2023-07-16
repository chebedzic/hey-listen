using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomBridge : Interactable
{
    public Vector3 offset;

    public void TryBridge()
    {
        HeroManager.instance.GetComponent<NavMeshAgent>().SetDestination(transform.position + offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, .2f);
        Gizmos.DrawLine(transform.position, transform.position + offset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + offset, .2f);
    }

    public override void OnMouseDown()
    {
        if(!interactable) return;
        TryBridge();
    }

    public override void OnMouseEnter()
    {
        if (!interactable) return;
        CursorHandler.instance.HoverInteractable(true, CursorType.navigate);
    }

    public override void OnMouseExit()
    {
        if (!interactable) return;
        CursorHandler.instance.HoverInteractable(false, CursorType.navigate);
    }

    private void OnDisable()
    {
        if(CursorHandler.instance != null)
            CursorHandler.instance.HoverInteractable(false, CursorType.navigate);
    }
}
