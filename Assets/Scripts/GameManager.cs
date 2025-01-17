using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnMenuRoomExit;

    public static GameManager instance;

    public RoomTrigger activeRoom;

    public bool hasUnlockedShield = false;
    public bool isPaused = false;

    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera focusVirtualCamera;

    [Header("Parameters")]
    [SerializeField] private float equipmentFocusTransition = .2f;
    [SerializeField] private float equipmentFocusInterval = 1;

    [SerializeField] private Image fadeImage;

    public Transform inventoryOrganizer;
    public GameObject inventorySlotPrefab;

    private void Awake()
    {
        instance = this;
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void GameIsLoading(bool loading)
    {
        EnableControls(!loading);
    }

    public void EnableControls(bool enable)
    {
        HeroManager.instance.isInteracting = !enable;

        PlayerInput[] playerInputs = FindObjectsByType<PlayerInput>(sortMode: FindObjectsSortMode.None);

        foreach (PlayerInput playerInput in playerInputs) { playerInput.SwitchCurrentActionMap(enable ? "Player" : "UI"); }

        CompanionManager.instance.enabled = enable;
    }

    public void FocusCameraOnObject(Transform target, bool focus, float transition = .2f, float interval = 0)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = transition;

        focusVirtualCamera.Follow = target;
        focusVirtualCamera.Priority = focus ? 11 : 0;

        if (interval > 0) StartCoroutine(Interval());

        IEnumerator Interval()
        {
            yield return new WaitForSeconds(interval);
            focusVirtualCamera.Priority = 0;
        }
    }


    public void GetEquipment(Equipment equipment)
    {
        PauseControlInterval(equipmentFocusInterval);

        FocusCameraOnObject(HeroManager.instance.transform, true, equipmentFocusTransition, equipmentFocusInterval);

        StartCoroutine(CollectEvent());

        IEnumerator CollectEvent()
        {
            if (equipment.type == EquipmentType.shield)
                HeroManager.instance.canMove = false;
            yield return new WaitForSeconds(equipmentFocusTransition);
            HeroManager.instance.SetHeroEquipment(equipment);
            HeroVisual.instance.newEquipmentParticle.Play();
            yield return new WaitForSeconds(equipmentFocusInterval);
            if (equipment.type == EquipmentType.shield)
            {
                hasUnlockedShield = true;
                HeroVisual.instance.SetEquipmentTutorial(true);
            }

        }

    }


    public void PauseControlInterval(float interval = 1)
    {
        StartCoroutine(IntervalCoroutine());

        IEnumerator IntervalCoroutine()
        {
            EnableControls(false);
            yield return new WaitForSeconds(interval);
            EnableControls(true);
        }
    }

    public void SetActiveRoom(RoomTrigger room)
    {

        RoomTrigger oldRoom = activeRoom;
        activeRoom = room;

        if (room == null || oldRoom == null)
        {
            Debug.LogWarning("Room not found");
            return;
        }

        if (!room.isMenuRoom && oldRoom.isMenuRoom)
        {
            OnMenuRoomExit.Invoke();
            GameTitleScreen.instance.BringTitleElements(false);
        }

        if (oldRoom != null)
            StartCoroutine(RoomDeactivationCooldown());

        IEnumerator RoomDeactivationCooldown()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(.1f);
            oldRoom.SetRoomActive(false);
        }

    }
    public void PauseGame(bool pause)
    {
        if (activeRoom.isMenuRoom)
            return;
        isPaused = pause;
        GameTitleScreen.instance.SetInitialDelay(0);
        GameTitleScreen.instance.BringTitleElements(isPaused);
        //EnableControls(!isPaused);
    }

    public void RestartGame()
    {
        AudioManager.instance.SetAmbientVolume(0);
        AudioManager.instance.SetMusicVolume(0,.5f);
        fadeImage.DOFade(1, 1).OnComplete(() =>
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0)
        );
    }

    public void QuitGame()
    {
        AudioManager.instance.SetAmbientVolume(0);
        AudioManager.instance.SetMusicVolume(0, .5f);
        fadeImage.DOFade(1, 1).OnComplete(() =>
        Application.Quit()
        );
    }

    public void AddInventorySlot()
    {
        GameObject inventory = Instantiate(inventorySlotPrefab, inventoryOrganizer);
        inventory.transform.localPosition = new Vector3(inventory.transform.position.x, inventory.transform.position.y, 0);
    }
}
