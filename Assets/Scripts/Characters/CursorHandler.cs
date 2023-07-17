using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public enum CursorType { arrow, hover, navigate}
public class CursorHandler : MonoBehaviour
{
    public static CursorHandler instance;

    [SerializeField] CursorSettings settings;
    private CompanionManager companion;
    private Interactable currentInteractable;
    private Image cursorImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        companion = FindObjectOfType<CompanionManager>();
        cursorImage = GetComponent<Image>();
        companion.OnMouseMovement.AddListener(FollowCursor);
        companion.OnMouseClick.AddListener(HandleClick);


        Cursor.visible = false;
        cursorImage.color = settings.arrowColor;
    }

    public void HoverInteractable(bool hover, CursorType type)
    {
        Sprite cursorSprite;
        switch (type)
        {
            case CursorType.hover:
                cursorSprite = settings.handCursor;
                break;
            case CursorType.navigate:
                cursorSprite = settings.navigateCursors[ChooseNavigateIndex()];

                break;
            default:
                cursorSprite = settings.handCursor;
                break;
        }   

        cursorImage.sprite = hover ? cursorSprite : settings.arrowCursor;
        //cursorImage.DOColor(hover ? settings.handColor : settings.arrowColor, .2f);
    }


    int ChooseNavigateIndex()
    {
        Vector3 mousePosition = Mouse.current.position.value;

        // Get the screen edges
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the distances to the screen edges
        float topDistance = screenHeight - mousePosition.y;
        float bottomDistance = mousePosition.y;
        float rightDistance = screenWidth - mousePosition.x;
        float leftDistance = mousePosition.x;

        // Find the minimum distance among the four directions
        float minDistance = Mathf.Min(topDistance, bottomDistance, rightDistance, leftDistance);

        // Choose the direction based on the minimum distance
        if (minDistance == topDistance)
        {
            return 0;
        }
        else if (minDistance == bottomDistance)
        {
            return 1;
        }
        else if (minDistance == rightDistance)
        {
            return 2;
        }
        else if (minDistance == leftDistance)
        {
            return 3;
        }

        return 0;
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