using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVisualHandler : MonoBehaviour
{

    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnClick.AddListener(PlayClickSound);
        interactable.OnPointerEnter.AddListener(PlayHoverSound);
    }

    void PlayClickSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_releaseAction, null);
    }

    void PlayHoverSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_hover, null);
    }


}
