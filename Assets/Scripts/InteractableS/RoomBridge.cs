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

    public override void Awake()
    {
        base.Awake();

        offMeshLink = GetComponentInParent<OffMeshLink>();

    }

    public void TryBridge()
    {
        if (linkedDoor != null) 
        { 
            if (!linkedDoor.offMeshLink.activated)
            {
                linkedDoor.offMeshLink.activated = true;
                StartCoroutine(LinkedDoor());
                return;
            }
        }

        HeroManager.instance.SetHeroDestination(transform.position + offset);
    }

    IEnumerator LinkedDoor()
    {
        linkedDoor.transform.GetChild(0).gameObject.SetActive(false);
        print("startedCoroutine");
        HeroManager.instance.SetHeroDestination(transform.position + offset);
        yield return new WaitForSeconds(.2f);
        yield return new WaitUntil(() => HeroManager.instance.AgentIsStopped());
        print("finished Coroutine");
        linkedDoor.transform.GetChild(0).gameObject.SetActive(true);

        linkedDoor.SetRelatedLink(false, true);
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
        if (offMeshLink != null)
            if (!offMeshLink.activated)
                return;

        TryBridge();
    }

    public override void OnMouseEnter()
    {
        CompanionManager.instance.currentInteractable = this;
        OnPointerEnter?.Invoke(true);

        if (offMeshLink != null)
            if (!offMeshLink.activated)
                return;

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

}
