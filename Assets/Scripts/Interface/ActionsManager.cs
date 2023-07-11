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
    [SerializeReference] private ScriptableRendererFeature renderFeature;
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

    public void TryCollectAction(Action action)
    {
        availableActions.Add(action);

        print("Grabbed: " + action.actionName + " - Material is:" + action.actionMaterial);

        InteractableUI actionUI = Instantiate(actionPrefab, transform).GetComponentInChildren<InteractableUI>();
        actionUI.Setup(action, action.actionMaterial);
    }
}
