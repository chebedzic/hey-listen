using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    public string roomName;
    public Action[] roomActions;
}
