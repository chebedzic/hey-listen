using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionsManager : MonoBehaviour
{
    private CompanionManager companionManager;
    public Action[] availableActions;
    [SerializeField] private GameObject actionPrefab;
    private RectTransform actionsHolderRect;

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
        actionsHolderRect.DOAnchorPosY(show ? -actionsHolderRect.sizeDelta.y : 0, .2f, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
