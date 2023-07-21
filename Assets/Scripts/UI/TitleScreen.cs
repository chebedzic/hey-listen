using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleScreen : MenuScreen
{
    [SerializeField] private LoadRooms loadRooms;
    [SerializeField] private Button playButton, settingsButton, quitButton;
    [SerializeField] private GameObject titleCamera;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        loadRooms.onLoadComplete.AddListener(TransitionToGame);
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveListener(PlayGame);
        quitButton.onClick.RemoveListener(QuitGame);
        loadRooms.onLoadComplete.RemoveListener(TransitionToGame);
    }
    private void Start()
    {
        SetActive(true);
    }
    private void PlayGame()
    {
        canvasGroup.interactable = false;
        loadRooms.Load();
    }
    private void TransitionToGame()
    {
        SetActive(false);
        Sequence introSequence = DOTween.Sequence()
            .AppendCallback(() => titleCamera.SetActive(false))
            .AppendInterval(2)
            .AppendCallback(() => loadRooms.Unload("TitleScreen"));
    }
    private void OpenSettings()
    {
    }
    private void QuitGame()
    {
        canvasGroup.interactable = false;
        Application.Quit();
    }
}
