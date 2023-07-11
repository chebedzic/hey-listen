using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    public bool enabled = true;
    [HideInInspector] public Renderer[] interactableRenderers;
    [HideInInspector] public bool selected;

    private void Awake()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
    }

    public void Highlight(bool state)
    {

        if (state)
        {
            transform.GetChild(0).DOComplete();
            transform.GetChild(0).DOShakeScale(.2f, .5f, 20, 20, true);
        }

        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if(mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
    }

    public virtual void ClickHandler()
    {
        CompanionManager.instance.ToggleEditMode();
    }

    private void OnDestroy()
    {
        transform.GetChild(0).DOComplete();
        transform.DOComplete();
    }


}
