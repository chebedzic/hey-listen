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
    [SerializeField] private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        interactableRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void Setup(Action action, Material actionMat)
    {
        if (action.actionMesh != null)
            if(meshFilter != null)
                meshFilter.mesh = action.actionMesh;

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


    public override void OnMouseDown()
    {
        base.OnMouseDown();

        CompanionManager.instance.selectedInteractable = this;
        rotationBeforeDrag = transform.eulerAngles;
        isBeingDragged = true;
        selected = true;
        GetComponent<Collider>().enabled = false;
    }

    public override void OnMouseExit()
    {
        if (selected)
            return;
        base.OnMouseExit();
    }

    public override void OnMouseDrag()
    {
        base.OnMouseDrag();

        Vector3 screenPoint = Mouse.current.position.value;
        screenPoint.z = 6; //distance of the plane from the camera
        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(screenPoint), 15 * Time.deltaTime);
    }

    public override void OnMouseUp()
    {
        base.OnMouseUp();

        if (CompanionManager.instance.currentSlot != null)
        {
            if (CompanionManager.instance.currentSlot.slotType == interfaceAction.actionType)
            {
                CompanionManager.instance.currentSlot.FillSlot(true, interfaceAction);
                CompanionManager.instance.currentSlot = null;
                Destroy(transform.parent.gameObject);
            }
            else
            {
                print("slot type not compatible");
            }
        }

        //return to origin

        CompanionManager.instance.selectedInteractable = null;
        isBeingDragged = false;
        selected = false;
        transform.DOLocalMove(Vector3.zero, .1f).SetEase(Ease.OutSine);
        transform.eulerAngles = rotationBeforeDrag;
        GetComponent<Collider>().enabled = true;
    }

}
