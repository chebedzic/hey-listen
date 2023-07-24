using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHero : Interactable
{
    public override void OnMouseDown()
    {
        if (!GameManager.instance.hasUnlockedShield)
            return;

        base.OnMouseDown();

        EquipmentManager.instance.ShowEquipments(!EquipmentManager.instance.visible);
    }

    public override void OnMouseEnter()
    {
        if (!GameManager.instance.hasUnlockedShield)
            return;

        base.OnMouseEnter();
    }
}
