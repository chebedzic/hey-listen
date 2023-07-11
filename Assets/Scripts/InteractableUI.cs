using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class InteractableUI : Interactable
{
    [SerializeField] private Action interfaceAction;

    private Vector3 rotationBeforeDrag;
    private bool isBeingDragged;

    [SerializeField] private Renderer actionRenderer;

    // Start is called before the first frame update
    void Start()
    {
        interactableRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void Setup(Action action, Material actionMat)
    {
        interfaceAction = action;
        actionRenderer.materials = new Material[] { actionRenderer.materials[0], actionMat };
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = Mouse.current.delta.value;

#if PLATFORM_STANDALONE_OSX

        delta = Mouse.current.delta.value * 10;
#endif

        if(isBeingDragged)
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, -delta.y, 10 * Time.deltaTime), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, delta.x, 10 * Time.deltaTime));
    }

    public override void ClickHandler()
    {
        //base.ClickHandler();
    }

    private void OnMouseDown()
    {
        rotationBeforeDrag = transform.eulerAngles;
        isBeingDragged = true;
        selected = true;
        GetComponent<Collider>().enabled = false;
    }

    private void OnMouseDrag()
    {
        Vector3 screenPoint = Mouse.current.position.value;
        screenPoint.z = 2; //distance of the plane from the camera
        transform.position = Vector3.Lerp(transform.position,Camera.main.ScreenToWorldPoint(screenPoint), 15 * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        if (CompanionManager.instance.currentSlot != null)
        {
            CompanionManager.instance.currentSlot.FillSlot(true, interfaceAction);
            Destroy(transform.parent.gameObject);
        }

        //return to origin

        isBeingDragged = false;
        selected = false;
        transform.DOLocalMove(Vector3.zero, .1f).SetEase(Ease.OutSine);
        transform.eulerAngles = rotationBeforeDrag;
        GetComponent<Collider>().enabled = true;
    }
}