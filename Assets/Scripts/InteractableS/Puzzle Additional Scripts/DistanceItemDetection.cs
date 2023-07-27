using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceItemDetection : MonoBehaviour
{
    public Item requiredItem;
    public UnityEvent OnItemDetected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Companion") && CompanionManager.instance.heldItem == requiredItem)
        {
            OnItemDetected.Invoke();
            CompanionManager.instance.SetHeldItem(null);
        }
    }
}
