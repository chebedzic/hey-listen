using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DistanceTrigger : MonoBehaviour
{
    InteractablePuzzle puzzle;

    void Start()
    {
        puzzle = GetComponentInParent<InteractablePuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            puzzle.TriggerPuzzle();
        }
    }

    private void OnDrawGizmos()
    {
        Color myColor = Color.green;
        myColor.a = .5f;
        Gizmos.color = myColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
