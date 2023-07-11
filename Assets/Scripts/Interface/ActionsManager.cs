using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager instance;

    private CompanionManager companionManager;
    public List<Action> availableActions;
    [SerializeField] private GameObject actionPrefab;
    private RectTransform actionsHolderRect;
    [SerializeField] private Renderer editModeQuad;

    [Header("Renderer Feature")]
    [SerializeReference] private UniversalRendererData rendererData;
    [SerializeReference] private ScriptableRendererFeature[] renderFeatures;
    [SerializeField] private LayerMask editModeMask;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        companionManager = CompanionManager.instance;
        actionsHolderRect = GetComponent<RectTransform>();
        companionManager.OnEditorMode.AddListener(ShowActions);

        //Setup();
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
            SetActiveStateOfRenderers(show);
            rendererData.opaqueLayerMask = editModeMask;
        }
        else
        {
            transform.DOMoveX(0, .2f).OnComplete(() => SetActiveStateOfRenderers(false));
        }

        actionsHolderRect.DOAnchorPosY(show ? -actionsHolderRect.sizeDelta.y : 0, .2f, false);

        editModeQuad.material.DOFade(show ? .8f : 0, .1f);
    }

    void SetActiveStateOfRenderers(bool show)
    {
        foreach (ScriptableRendererFeature rendererFeature in renderFeatures)
            rendererFeature.SetActive(show);

        if (!show)
        {
            rendererData.opaqueLayerMask = ~0; 
            rendererData.transparentLayerMask = ~0; 
        }
    }

    private void OnDestroy()
    {
        SetActiveStateOfRenderers(false);
        rendererData.opaqueLayerMask = ~0;
        rendererData.transparentLayerMask = ~0;

    }

    public void TryCollectAction(Action action)
    {
        availableActions.Add(action);

        print("Grabbed:<b><color=#"+ColorUtility.ToHtmlStringRGB(action.actionColor)+"> "+ action.actionName + "</b></color>");

        InteractableUI actionUI = Instantiate(actionPrefab, transform).GetComponentInChildren<InteractableUI>();
        actionUI.Setup(action, action.actionMaterial);
        actionUI.transform.parent.DOShakeScale(.2f, .5f, 20, 90, true);
    }
}
