using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEquipment : Interactable
{
    public bool bubble = true;
    public Equipment equipment;

    public float collectEventDelay = 1.5f;
    public UnityEvent AfterEquipmentCollect;

    public override void Awake()
    {
        base.Awake();
    }

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

            GameManager.instance.PauseControlInterval(2);

            transform.GetChild(0).gameObject.SetActive(false);

            GameManager.instance.FocusCameraOnObject(HeroManager.instance.transform,true, .5f, 2);

            StartCoroutine(CollectEvent());

            IEnumerator CollectEvent()
            {
                yield return new WaitForSeconds(.5f);
                HeroManager.instance.SetHeroEquipment(equipment);

                yield return new WaitForSeconds(collectEventDelay);
                AfterEquipmentCollect.Invoke();
                gameObject.SetActive(false);
            }
        }
        
    }
}
