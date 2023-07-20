using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DebugHero : MonoBehaviour
{
    InteractableModal[] modals;

    [SerializeField] float debugMovementDistance = 2;

    void OnA()
    {
        DebugMoveHero(-Vector3.right);
    }

    void OnS()
    {
        DebugMoveHero(-Vector3.forward);
    }

    void OnW()
    {
        DebugMoveHero(Vector3.forward);

    }

    void OnD()
    {
        DebugMoveHero(Vector3.right);
    }

    void OnM()
    {
        foreach (InteractableModal modal in FindObjectsByType<InteractableModal>(FindObjectsInactive.Include, FindObjectsSortMode.None)) 
        {
            modal.gameObject.SetActive(!modal.gameObject.activeSelf);
        }
    }

    void OnMuteMusic()
    {
        AudioManager.instance.SetMusicVolume(0);
    }

    void DebugMoveHero(Vector3 dir)
    {
        HeroManager.instance.GetComponent<NavMeshAgent>().enabled = false;
        HeroManager.instance.transform.position += dir * debugMovementDistance;
        HeroManager.instance.GetComponent<NavMeshAgent>().enabled = true;
    }
}
