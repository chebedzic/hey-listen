using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVisualHandler : MonoBehaviour
{

    private Interactable interactable;
    private InteractableCollectable collectable;

    [SerializeField] private bool shakeOnHover = true;
    [SerializeField] private bool soundOnHover = true;
    private Renderer[] interactableRenderers;


    // Start is called before the first frame update
    void Awake()
    {

        interactable = GetComponent<Interactable>();
        collectable = GetComponent<InteractableCollectable>();

        interactableRenderers = GetComponentsInChildren<Renderer>();
        interactable.OnClick.AddListener(PlayClickSound);
        interactable.OnPointerEnter.AddListener(HoverVisual);

        if(collectable)
            collectable.OnCollect.AddListener(CollectVisual);
    }


    public void HoverVisual(bool state)
    {
        Highlight(state, interactableRenderers);

        if (state)
            PlayHoverSound();
    }

    public void JumpToTransform(Transform reference)
    {
        transform.DOComplete();
        transform.DOLocalJump(reference.transform.localPosition, 2, 1, .5f, false);
    }

    void CollectVisual()
    {
        //if (transform.childCount > 0)
        //{
        //    transform.GetChild(0).DOComplete();
        //    transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 20, true);
        //}

    }

    void PlayClickSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_releaseAction, null);
    }

    void PlayHoverSound()
    {
        if (!soundOnHover)
            return;

        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.UI_hover, null);
    }

    public void PlaySound()
    {

    }
    public void ForceHighlight(bool state, float amount = .5f)
    {
        if (interactableRenderers == null)
            return;

        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasFloat("_ForceFresnel"))
                    mat.DOFloat(state ? amount : 0, "_ForceFresnel", .2f);
            }
        }
    }
    protected virtual void Highlight(bool state, Renderer[] interactableRenderers)
    {

        if (state && transform.childCount > 0 && shakeOnHover)
        {
            transform.GetChild(0).DOComplete();
            transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 20, true);
        }

        if (interactableRenderers == null)
            return;

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
