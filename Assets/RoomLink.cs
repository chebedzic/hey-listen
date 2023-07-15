using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class RoomLink : MonoBehaviour
{

    [SerializeField] private GeneralSettings generalSettings;
    [SerializeField] private BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger with " + other.name);

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineVirtualCamera virtualCam = (CinemachineVirtualCamera)brain.ActiveVirtualCamera;
        Vector3 roomCamPlacement = new Vector3(transform.parent.position.x, virtualCam.transform.position.y, transform.parent.position.z - 6);
        virtualCam.transform.DOComplete();
        virtualCam.transform.DOMove(roomCamPlacement, generalSettings.roomCameraTransitionSpeed, false);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.blue;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }

}
