using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Action")]
public class Action : ScriptableObject
{
    public string actionName;
    public Material actionMaterial;
    public Color actionColor = Color.red;
    public Mesh actionMesh;
    public Sprite actionIcon;
    public ActionType actionType;
}
