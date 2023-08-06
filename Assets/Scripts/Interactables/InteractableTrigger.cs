using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class InteractableTrigger : Interactable
{
    public UnityEvent onTriggerEnter;
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hero"))
        {
            onTriggerEnter.Invoke();
        }
    }
}
