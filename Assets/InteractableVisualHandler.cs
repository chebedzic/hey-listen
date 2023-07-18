using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVisualHandler : MonoBehaviour
{

    private Interactable interactable;

    [SerializeField] private bool shakeOnHover = true;
    private Renderer[] interactableRenderers;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactable.OnClick.AddListener(PlayClickSound);
        interactable.OnPointerEnter.AddListener(HoverVisual);
    }


    void HoverVisual(bool state)
    {
        Highlight(state, interactableRenderers);

        if (state)
            PlayHoverSound();
    }

    void PlayClickSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_releaseAction, null);
    }

    void PlayHoverSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_hover, null);
    }

    public void PlaySound()
    {

    }
    public virtual void Highlight(bool state, Renderer[] interactableRenderers)
    {

        if (state && transform.childCount > 0 && shakeOnHover)
        {
            transform.GetChild(0).DOComplete();
            transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 20, true);
        }

        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
    }
}
