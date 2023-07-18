using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTrigger : MonoBehaviour
{
    Collider distanceTrigger;
    InteractablePuzzle puzzle;

    // Start is called before the first frame update
    void Start()
    {
        distanceTrigger = GetComponent<Collider>();
        puzzle = GetComponentInParent<InteractablePuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            puzzle.TriggerPuzzle();
        }
    }
}
