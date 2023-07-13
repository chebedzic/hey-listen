using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combination")]
public class ActionCombination : ScriptableObject
{
    public List<Action> actions;
    public string trigger;
}
