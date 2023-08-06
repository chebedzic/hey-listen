using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class GameEndingScreen : MonoBehaviour
{
    public static GameEndingScreen instance;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup buttonGroup;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        instance = this;
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas.enabled = false;
        
    }

    public void ShowEndingScreen()
    {
        fadeGroup.alpha = 0;
        
        canvas.enabled = true;
        canvasGroup.interactable = true;

        DOTween.Sequence()
        .Append(canvasGroup.DOFade(1, 1f).From(0))
        .Join(titleGroup.DOFade(1, 0.3f).SetDelay(0.7f).From(0))
        .AppendInterval(1f)
        .Append(buttonGroup.DOFade(1, 0.5f).From(0));

    }
    public void RestartGame()
    {
        canvasGroup.interactable = false;

        DOTween.Sequence()
        .Append(fadeGroup.DOFade(1, 1f))
        .Join(titleGroup.DOFade(0, 0.3f))
        .Join(buttonGroup.DOFade(0, 0.3f))
        .OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
    public void QuitGame()
    {
        canvasGroup.interactable = false;
          DOTween.Sequence()
        .Append(fadeGroup.DOFade(1, 1f))
        .Join(titleGroup.DOFade(0, 0.3f))
        .Join(buttonGroup.DOFade(0, 0.3f))
        .OnComplete(() =>
        {
            Application.Quit();
        });
    }

}
