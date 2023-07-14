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
    [SerializeField][ColorUsage(true, true)] Color editModeColor;
    [SerializeField] private float editModeOffset = 3;

    private float offset;

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

    private void Update()
    {
        if (!CompanionManager.instance.isInEditorMode)
            return;

        offset += editModeOffset * Time.deltaTime;

        editModeQuad.material.mainTextureOffset = new Vector2(offset, 0);
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
        if (!show)
            transform.DOMoveX(0, .2f);

        actionsHolderRect.DOAnchorPosY(show ? -actionsHolderRect.sizeDelta.y : 0, .2f, false);

        //editModeQuad.material.DOFade(show ? .8f : 0, .1f);
        editModeQuad.material.DOColor(show ? editModeColor : Color.black,"_EmissionColor", .1f);
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
