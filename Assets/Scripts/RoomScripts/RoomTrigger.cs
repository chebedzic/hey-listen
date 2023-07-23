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

    [HideInInspector] public Interactable[] roomInteractions;
    [HideInInspector] public GameObject roomCompanionSurface;

    private void Awake()
    {
        // Look for all interactables that are part of the parent room
        roomInteractions = transform.parent.GetComponentsInChildren<Interactable>();

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

    }

    //TODO replace this to detect when levels where loaded
    private void Start()
    {
        StartCoroutine(WaitSecondForDeactivation());

        IEnumerator WaitSecondForDeactivation()
        {
            yield return new WaitForSeconds(1);
            if(GameManager.instance.activeRoom != this)
                SetRoomActive(false);
        }
    }


    public void SetRoomActive(bool active)
    {
        if(active)
            GameManager.instance.SetActiveRoom(this);

        roomCompanionSurface.SetActive(active);
        foreach (Interactable interaction in roomInteractions)
        {
            if (interaction != null)
            {
                if (!interaction.CompareTag("Hero") && interaction != this)
                {
                    interaction.SetColliderState(active);
                }
            }
        }

        Interactable[] dynamicInteractables = GetComponentsInChildren<Interactable>();

        if(dynamicInteractables.Length > 0)
        {
            foreach (Interactable interaction in dynamicInteractables)
            {
                if (interaction != null)
                    interaction.SetColliderState(active);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero") && GameManager.instance.activeRoom != this)
        {
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            CinemachineVirtualCamera virtualCam = (CinemachineVirtualCamera)brain.ActiveVirtualCamera;
            Vector3 roomCamPlacement = new Vector3(transform.parent.position.x, virtualCam.transform.position.y, transform.parent.position.z - 6);
            virtualCam.transform.DOComplete();
            virtualCam.transform.DOMove(roomCamPlacement, generalSettings.roomCameraTransitionSpeed, false);

            //RoomManager.instance.RoomSetup(this);
            SetRoomActive(true);
        }
    }


    private void OnDrawGizmos()
    {
        Color color = Color.blue;
        color.a = .2f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }

}
