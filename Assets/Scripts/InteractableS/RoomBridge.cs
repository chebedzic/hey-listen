using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class RoomBridge : Interactable
{
    public Vector3 offset;
    private OffMeshLink offMeshLink;

    public InteractablePuzzle linkedDoor;
    Coroutine InteractionCooldown;

    public override void Awake()
    {
        base.Awake();

        offMeshLink = GetComponentInParent<OffMeshLink>();

    }

    public void TryBridge()
    {
        if (HeroManager.instance.isLookingForBridge)
            return;

        HeroManager.instance.isLookingForBridge = true;

        if (linkedDoor != null) 
        { 
            if (!linkedDoor.offMeshLink.activated)
            {
                print("backToRoom");
                linkedDoor.BackToRoom(transform.position+offset);
                return;
            }
        }

        HeroManager.instance.SetHeroDestination(transform.position + offset);

        if(InteractionCooldown!= null) StopCoroutine(InteractionCooldown);
        InteractionCooldown = StartCoroutine(Cooldown());

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(.5f);
            yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
            yield return new WaitUntil(() => !HeroManager.instance.IsAgentCrossingLink());
            HeroManager.instance.isLookingForBridge = false;
            if(!HeroManager.instance.isInteracting)
            HeroManager.instance.SetHeroDestination(HeroManager.instance.transform.position + (HeroManager.instance.transform.forward/2));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, .2f);
        Gizmos.DrawLine(transform.position, transform.position + offset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + offset, .2f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + (Vector3.up * 5),.4f);
    }

    public override void OnMouseDown()
    {
        if (HeroManager.instance.isInteracting)
            return;

        if (HeroManager.instance.isLookingForBridge)
            return;

        if (offMeshLink != null)
            if (!offMeshLink.activated)
                return;


        TryBridge();

        //OnMouseExit();
    }

    public override void OnMouseEnter()
    {
        if(HeroManager.instance.IsAgentCrossingLink()) return;

        CompanionManager.instance.currentInteractable = this;

        if (offMeshLink != null)
            if (!offMeshLink.activated)
                return;

        OnPointerEnter?.Invoke(true);
        CursorHandler.instance.HoverInteractable(true, CursorType.navigate);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();

        if (offMeshLink != null)
            if (!offMeshLink.activated)
                return;

        CursorHandler.instance.HoverInteractable(false, CursorType.navigate);
    }

    private void OnDisable()
    {
        if(CursorHandler.instance != null)
            CursorHandler.instance.HoverInteractable(false, CursorType.navigate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractablePuzzle>() != null)
        {
            if (other.CompareTag("Door"))
            {
                if (transform.CompareTag("Gap"))
                    return;
                linkedDoor = other.GetComponent<InteractablePuzzle>();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InteractablePuzzle>() != null && linkedDoor == null)
        {
            if (other.CompareTag("Door"))
            {
                if (transform.CompareTag("Gap"))
                    return;
                linkedDoor = other.GetComponent<InteractablePuzzle>();
            }
        }
    }

}
