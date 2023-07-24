using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEquipment : Interactable
{
    public bool bubble = true;
    public Equipment equipment;

    public override void OnMouseEnter()
    {
        if (!bubble) return;
        base.OnMouseEnter();
        CompanionManager.instance.currentEquipmentBubble = this;
    }

    public override void OnMouseExit()
    {
        if (!bubble) return;
        base.OnMouseExit();
        CompanionManager.instance.currentEquipmentBubble = null;

    }


    public override void OnMouseDown()
    {
        if (!bubble) return;
        base.OnMouseDown();

        if(equipment == null) { print("Equipment Object Null"); }
        HeroManager.instance.SetHeroEquipment(equipment);
        EquipmentManager.instance.ShowEquipments(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bubble)
            return;

        if (other.CompareTag("Hero"))
        {
            if (equipment == null) { print("Equipment Object Null"); }
            HeroManager.instance.SetHeroDestination(HeroManager.instance.transform.position);
            HeroManager.instance.SetHeroEquipment(equipment);
            EquipmentManager.instance.ShowEquipments(false);
            GameManager.instance.hasUnlockedShield = true;
            gameObject.SetActive(false);
        }
        
    }
}
