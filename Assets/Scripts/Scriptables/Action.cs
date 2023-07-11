using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Action")]
public class Action : ScriptableObject
{
    public enum ActionType { input, modifier };
    public string actionName;
    public Material actionMaterial;
    public Sprite actionIcon;
    public ActionType actionType;
}
