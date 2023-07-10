using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] CursorSettings settings;
    private CompanionManager companion;
    private Interactable currentInteractable;
    private Image cursorImage;

    private void Start()
    {

        companion = FindObjectOfType<CompanionManager>();
        cursorImage = GetComponent<Image>();
        companion.OnHoverInteractable.AddListener(HoverInteractable);
        companion.OnMouseMovement.AddListener(FollowCursor);
        companion.OnMouseClick.AddListener(HandleClick);


        Cursor.visible = false;
        cursorImage.color = settings.arrowColor;
    }

    void HoverInteractable(bool hover)
    {
        cursorImage.sprite = hover ? settings.handCursor : settings.arrowCursor;
        cursorImage.DOColor(hover ? settings.handColor : settings.arrowColor, .2f);
    }

    void FollowCursor(Vector3 pos)
    {
        cursorImage.transform.position = pos;
    }

    void HandleClick(Vector3 worldPosition)
    {
        if (Cursor.visible)
            Cursor.visible = false;

        ClickAnimation();
    }

    void ClickAnimation()
    {
        transform.DOComplete();
        transform.DOPunchScale(-Vector3.one/4, .4f, 10, 1);
    }


}