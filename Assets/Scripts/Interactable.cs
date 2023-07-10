using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    private Renderer[] interactableRenderers;

    private void Start()
    {
        interactableRenderers = GetComponentsInChildren<Renderer>();
    }

    public void Highlight(bool state)
    {
        foreach (Renderer renderer in interactableRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if(mat.HasFloat("_FresnelAmount"))
                    mat.DOFloat(state ? 1 : 0, "_FresnelAmount", .2f);
            }

        }
    }
}
