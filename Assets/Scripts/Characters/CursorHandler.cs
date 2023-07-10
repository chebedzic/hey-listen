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
        Cursor.visible = false;

        companion = FindObjectOfType<CompanionManager>();
        cursorImage = GetComponent<Image>();
        companion.OnHoverInteractable.AddListener(HoverInteractable);
        companion.OnMouseMovement.AddListener(FollowCursor);
        companion.OnMouseClick.AddListener(ClickAnimation);
    }

    void HoverInteractable(bool hover)
    {
        cursorImage.sprite = hover ? settings.handCursor : settings.arrowCursor;
    }

    void FollowCursor(Vector3 pos)
    {
        cursorImage.transform.position = pos;
    }

    void ClickAnimation()
    {
        transform.DOComplete();
        transform.DOPunchScale(-Vector3.one/4, .4f, 10, 1);
    }


}