using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    public bool visible;

    private void Awake()
    {
        instance= this;

        ShowEquipments(false);
    }

    public void ShowEquipments(bool state)
    {
        visible = state;
        transform.GetChild(0).gameObject.SetActive(state);
    }
}
