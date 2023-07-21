using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public RoomTrigger activeRoom;

    private void Awake()
    {
        instance = this;
    }

    public void SetActiveRoom(RoomTrigger room)
    {
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
