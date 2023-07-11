using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class InteractableUI : Interactable
{
    private Vector3 rotationBeforeDrag;
    private bool isBeingDragged;

    // Start is called before the first frame update
    void Start()
    {
        interactableRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingDragged)
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, -Mouse.current.delta.value.y, 10 * Time.deltaTime), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, Mouse.current.delta.value.x, 10 * Time.deltaTime));
    }

    private void OnMouseDown()
    {
        rotationBeforeDrag = transform.eulerAngles;
        isBeingDragged = true;
        selected = true;
    }

    private void OnMouseDrag()
    {
        Vector3 screenPoint = Mouse.current.position.value;
        screenPoint.z = 2; //distance of the plane from the camera
        transform.position = Vector3.Lerp(transform.position,Camera.main.ScreenToWorldPoint(screenPoint), 15 * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        selected = false;
        transform.DOLocalMove(Vector3.zero, .1f).SetEase(Ease.OutSine);
        transform.eulerAngles = rotationBeforeDrag;
    }
}
