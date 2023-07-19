using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHero : Interactable
{
    public override void OnMouseDown()
    {
        base.OnMouseDown();

        EquipmentManager.instance.ShowEquipments(!EquipmentManager.instance.visible);
    }
}
