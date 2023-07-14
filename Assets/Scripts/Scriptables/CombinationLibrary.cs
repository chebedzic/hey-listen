using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { input, modifier };
[CreateAssetMenu(menuName = "Scriptable Objects/Combination Library")]
public class CombinationLibrary : ScriptableObject
{
    public ActionCombination[] combinations;

    public ActionCombination GetCombination(List<Action> actions)
    {
        foreach (ActionCombination combination in combinations)
        {
            if (CompareLists.AreEqual(combination.actions, actions))
            {
                return combination;
            }
        }

        Debug.LogWarning("No combination found!");
        return null;
    }
}
