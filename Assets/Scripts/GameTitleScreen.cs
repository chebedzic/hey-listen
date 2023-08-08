using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTitleScreen : MonoBehaviour
{
    public static GameTitleScreen instance;

    [SerializeField] private RectTransform logoRect;
    [SerializeField] private Image logoBg;
    [SerializeField] private RectTransform optionsRect;
    [SerializeField] private CanvasGroup instructionsCanvas;

    [Header("Settings")]
    [SerializeField] private float transitionDuration = .4f;
    [SerializeField] private float initialDelay = .5f;
    [SerializeField] private Ease transitionEase = Ease.OutBack;
    [SerializeField] private float logoBgAlpha = .5f;

    private void Awake()
    {
        instance = this;
    }

    public void BringTitleElements(bool active)
    {
        float delay = active ? initialDelay : 0f;

        logoRect.DOAnchorPosX(active ? logoRect.sizeDelta.x : 0, transitionDuration, false).SetEase(transitionEase).SetDelay(delay);
        optionsRect.DOAnchorPosX(active ? -optionsRect.sizeDelta.x : 0, transitionDuration, false).SetEase(transitionEase).SetDelay(delay);
        logoBg.DOFade(active ? logoBgAlpha : 0, transitionDuration).SetDelay(delay);
        instructionsCanvas.DOFade(active ? 1 : 0, transitionDuration).SetDelay(delay * 4);
    }
    public void Unpause()
    {
        GameManager.instance.PauseGame(false);
    }
}
