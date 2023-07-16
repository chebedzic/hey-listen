using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public static RoomManager instance;

    public List<RoomTrigger> rooms;
    [SerializeField] private RoomTrigger currentRoom;

    private void Awake()
    {
        instance= this;
    }

    void Start()
    {

    }

    public void RoomSetup(RoomTrigger incomingRoom)
    {
        if(incomingRoom != currentRoom)
        {
            foreach (RoomTrigger room in rooms)
            {
                room.SetRoomActive(false);
            }
            currentRoom = incomingRoom;
            currentRoom.SetRoomActive(true);
        }
    }
}
