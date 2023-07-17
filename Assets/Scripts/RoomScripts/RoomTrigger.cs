using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.AI;

public class RoomTrigger : MonoBehaviour
{

    [HideInInspector] public UnityEvent<RoomTrigger> OnRoomEnter;

    [SerializeField] private GeneralSettings generalSettings;
    [SerializeField] private BoxCollider collider;

    [HideInInspector] public RoomBridge[] roomBridges;
    [HideInInspector] public GameObject roomCompanionSurface;

    private void Awake()
    {
        // Look for all interactables that are part of the parent room
        roomBridges = transform.parent.GetComponentsInChildren<RoomBridge>();

        // Look for all sibling objects
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            //Find the one with the companion tag to set as the companion surface
            if (transform.parent.GetChild(i).CompareTag("CompanionSurface"))
                roomCompanionSurface = transform.parent.GetChild(i).gameObject;

            //If there is a camera object in the room, disable it
            if(transform.parent.GetChild(i).GetComponent<Camera>() != null)
                transform.parent.GetChild(i).gameObject.SetActive(false);
        }

        // Deactivate everything by default
        SetRoomActive(false);

    }

    public void SetRoomActive(bool active)
    {
        roomCompanionSurface.SetActive(active);
        foreach (RoomBridge bridge in roomBridges)
        {
            if (bridge.CanBeActiveOnStart() && bridge.interactableCollider != null)
                bridge.interactableCollider.enabled = active;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineVirtualCamera virtualCam = (CinemachineVirtualCamera)brain.ActiveVirtualCamera;
        Vector3 roomCamPlacement = new Vector3(transform.parent.position.x, virtualCam.transform.position.y, transform.parent.position.z - 6);
        virtualCam.transform.DOComplete();
        virtualCam.transform.DOMove(roomCamPlacement, generalSettings.roomCameraTransitionSpeed, false);

        //RoomManager.instance.RoomSetup(this);
        SetRoomActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetRoomActive(false);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.blue;
        color.a = .2f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }

}
