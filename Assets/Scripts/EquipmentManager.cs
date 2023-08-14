using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    
    public bool visible;
    public RectTransform equipmentHolder;
    public RectTransform canvas;

    public float topThreshold = 0.8f; // Threshold for determining if the player is closer to the center/top.
    public float bottomThreshold = 0.2f; // Threshold for determining if the player is at the bottom.
    public float centerOffset = -100f; // Offset when the player is closer to the center/top.
    public float bottomOffset = 200f; // Offset when the player is at the bottom.

    private void Awake()
    {
        instance= this;
        ShowEquipments(false);
    }

    public void ShowEquipments(bool state)
    {

        StartCoroutine(Cooldown());

        IEnumerator Cooldown()
        {
#if UNITY_WEBGL
            yield return new WaitForSeconds(state ? 0 : .1f);
#endif
            yield return new WaitForEndOfFrame();
            visible = state;

            equipmentHolder.gameObject.SetActive(state);

            for (int i = 0; i < equipmentHolder.childCount; i++)
            {
                equipmentHolder.GetChild(i).DOComplete();
                equipmentHolder.GetChild(i).DOScale(0, .15f).From().SetEase(Ease.OutBack).SetDelay(.1f * i);
            }

            Vector3 heroPos = HeroManager.instance.transform.position;

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(heroPos);
            float yOffset = (ViewportPosition.y <= bottomThreshold) ? bottomOffset : centerOffset;
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f)) + yOffset);

            //now you can set the position of the ui element
            equipmentHolder.anchoredPosition = WorldObject_ScreenPosition;
        }

    }
}
