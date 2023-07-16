using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{

    [HideInInspector] public UnityEvent<RoomTrigger> OnRoomEnter;

    [SerializeField] private GeneralSettings generalSettings;
    [SerializeField] private BoxCollider collider;

    [HideInInspector] public Interactable[] roomInteractables;
    [HideInInspector] public ModalScript[] roomModals;
    [HideInInspector] public GameObject roomCompanionSurface;

    private void Awake()
    {
        roomInteractables = GetComponentsInChildren<Interactable>();
        roomModals = GetComponentsInChildren<ModalScript>();
        roomCompanionSurface = transform.GetChild(0).gameObject;

        SetRoomActive(false);
    }

    public void SetRoomActive(bool active)
    {
        foreach ( Interactable interactable in roomInteractables )
        {
            interactable.enabled = active;
        }

        foreach(ModalScript modal in roomModals)
        {
            modal.enabled = active;
        }

        roomCompanionSurface.SetActive(active);
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
