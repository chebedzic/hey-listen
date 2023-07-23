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

#if UNITY_EDITOR
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


    void OnI()
    {
        HeroManager.instance.isInteracting = false;
    }

    void DebugMoveHero(Vector3 dir)
    {
        HeroManager.instance.GetComponent<NavMeshAgent>().enabled = false;
        HeroManager.instance.transform.position += dir * debugMovementDistance;
        HeroManager.instance.GetComponent<NavMeshAgent>().enabled = true;
    }


#endif

    void OnMuteMusic()
    {
        AudioManager.instance.SetMusicVolume(0);
    }

    void OnM()
    {
        foreach (InteractableModal modal in FindObjectsByType<InteractableModal>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            modal.gameObject.SetActive(!modal.gameObject.activeSelf);
        }
    }

}
