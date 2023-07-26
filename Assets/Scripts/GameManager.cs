using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnMenuRoomExit;

    public static GameManager instance;

    public RoomTrigger activeRoom;

    public bool hasUnlockedShield = false;

    private void Awake()
    {
        instance = this;
    }

    public void GameIsLoading(bool loading)
    {
        PlayerInput[] playerInputs = FindObjectsByType<PlayerInput>(sortMode: FindObjectsSortMode.None);
        
        foreach (PlayerInput playerInput in playerInputs) { playerInput.enabled = !loading; }

        CompanionManager.instance.enabled = !loading;
    }

    public void SetActiveRoom(RoomTrigger room)
    {
        if (!room.isMenuRoom)
        {
            //OnMenuRoomExit.Invoke();
            GameTitleScreen.instance.BringTitleElements(false);
        }

        RoomTrigger oldRoom = activeRoom;
        activeRoom = room;

        if (oldRoom != null)
            StartCoroutine(RoomDeactivationCooldown());

        IEnumerator RoomDeactivationCooldown()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(.1f);
            oldRoom.SetRoomActive(false);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
