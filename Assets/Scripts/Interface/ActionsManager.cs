using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class ActionsManager : MonoBehaviour
{
    private CompanionManager companionManager;
    public Action[] availableActions;
    [SerializeField] private GameObject actionPrefab;
    private RectTransform actionsHolderRect;
    [SerializeField] private Renderer editModeQuad;

    [Header("Renderer Feature")]
    [SerializeReference] private UniversalRendererData rendererData;
    [SerializeReference] private ScriptableRendererFeature renderFeature;
    [SerializeField] private LayerMask editModeMask;

    // Start is called before the first frame update
    void Start()
    {

        companionManager = FindObjectOfType<CompanionManager>();
        actionsHolderRect = GetComponent<RectTransform>();
        companionManager.OnEditorMode.AddListener(ShowActions);

        Setup();
    }

    void Setup()
    {
        foreach (Action action in availableActions)
        {
            GameObject actionClickable = Instantiate(actionPrefab, transform);
            //actionClickable.GetComponent<Image>().sprite = action.actionIcon;
        }
    }

    void ShowActions(bool show)
    {
        if (show)
        {
            renderFeature.SetActive(show);
            rendererData.opaqueLayerMask = editModeMask;
        }
        else
        {
            transform.DOMoveX(0, .2f).OnComplete(() => renderFeature.SetActive(false)); rendererData.opaqueLayerMask = ~0 ;
        }

        actionsHolderRect.DOAnchorPosY(show ? -actionsHolderRect.sizeDelta.y : 0, .2f, false);

        editModeQuad.material.DOFade(show ? .8f : 0, .2f);
    }

    private void OnDestroy()
    {
        renderFeature.SetActive(false);
        rendererData.opaqueLayerMask = ~0;

    }
}
