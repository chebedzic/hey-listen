using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] CursorSettings settings;
    private CompanionManager companion;
    private Interactable currentInteractable;

    private void Start()
    {
        companion = GetComponent<CompanionManager>();
        companion.OnHoverInteractable.AddListener(HoverInteractable);
    }

    void HoverInteractable(bool hover)
    {
        SetCursorImage(hover ? settings.handCursor : settings.arrowCursor);
    }

    void SetCursorImage(Texture2D texture2D)
    {
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }

}