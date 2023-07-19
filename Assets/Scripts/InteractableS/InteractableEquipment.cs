using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEquipment : Interactable
{

    public Equipment equipment;

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        CompanionManager.instance.currentEquipmentBubble = this;
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        CompanionManager.instance.currentEquipmentBubble = null;

    }


    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if(equipment == null) { print("Equipment Object Null"); }
        HeroManager.instance.SetHeroEquipment(equipment);
        EquipmentManager.instance.ShowEquipments(false);
    }
}
