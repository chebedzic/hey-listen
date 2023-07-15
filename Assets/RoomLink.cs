using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class RoomLink : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Trigger with " + other.name);

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineVirtualCamera virtualCam = (CinemachineVirtualCamera)brain.ActiveVirtualCamera;
        Vector3 roomCamPlacement = new Vector3(transform.parent.position.x, virtualCam.transform.position.y, transform.parent.position.z - 6);
        virtualCam.transform.DOComplete();
        virtualCam.transform.DOMove(roomCamPlacement, 1, false);
    }


}
