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
            GameManager.instance.FocusCameraOnObject(CompanionManager.instance.transform, true, .5f, 2.4f);
            GameManager.instance.PauseControlInterval(2.4f);

            StartCoroutine(Interval());

            IEnumerator Interval()
            {
                yield return new WaitForSeconds(.5f);
                OnItemDetected.Invoke();
                CompanionManager.instance.SetHeldItem(null);

            }
        }
    }
}
