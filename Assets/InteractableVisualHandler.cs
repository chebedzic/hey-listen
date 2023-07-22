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

    private Light[] interactableLights;

    // Start is called before the first frame update
    void Awake()
    {

        interactable = GetComponent<Interactable>();
        collectable = GetComponent<InteractableCollectable>();
        interactableLights = GetComponentsInChildren<Light>();

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

    public void JumpToCenter(float delay = 1)
    {
        transform.DOComplete();
        //transform.DORotate(new Vector3(360, 0, 0), .8f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).SetDelay(delay);
        transform.DOLocalJump(Vector3.zero, 2, 1, .5f, false).SetDelay(delay);
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
    public void ForceHighlight(bool state)
    {
        if (interactableRenderers == null)
            return;

        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasFloat("_ForceFresnel"))
                    mat.DOFloat(state ? 1 : 0, "_ForceFresnel", .2f);
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

        if (interactableLights.Length > 0)
        {
            foreach(Light light in interactableLights)
            {
                light.DOIntensity(state ? 7 : 0, .5f);
            }
        }
    }
}
