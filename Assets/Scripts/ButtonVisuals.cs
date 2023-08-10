using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler
{
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverScaleDuration = .2f;
    [SerializeField] private Ease hoverScaleEase = Ease.OutBack;

    [SerializeField] private Vector3 submitShake = -Vector3.one;
    [SerializeField] private float submitShakeDuration = .4f;
    [SerializeField] private int submitShakeVibrato = 15;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorHandler.instance.HoverInteractable(true, CursorType.hover);
        transform.DOComplete();
        transform.DOScale(hoverScale, hoverScaleDuration).SetEase(hoverScaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorHandler.instance.HoverInteractable(false, CursorType.arrow);
        transform.DOComplete();
        transform.DOScale(1, hoverScaleDuration).SetEase(hoverScaleEase);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOComplete();
        transform.DOPunchScale(submitShake, submitShakeDuration, submitShakeVibrato, 1);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        transform.DOComplete();
        transform.DOPunchScale(submitShake, submitShakeDuration, submitShakeVibrato, 1);
    }
}
