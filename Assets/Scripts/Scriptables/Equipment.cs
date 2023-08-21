using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { sword, shield, torch, inventory}

[CreateAssetMenu(menuName = "Scriptable Objects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentLabel;
    public EquipmentType type;
}
