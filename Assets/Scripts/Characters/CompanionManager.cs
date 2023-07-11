using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using DG.Tweening;

public class CompanionManager : MonoBehaviour
{

    public static CompanionManager instance;

    //Events
    [HideInInspector] public UnityEvent<bool> OnHoverInteractable;
    [HideInInspector] public UnityEvent<Vector3> OnMouseMovement;
    [HideInInspector] public UnityEvent<Vector3> OnMouseClick;
    [HideInInspector] public UnityEvent<bool> OnEditorMode;

    [Header("Parameters")]
    [SerializeField] private float mouseLerp = 10;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private LayerMask groundLayerMask;

    private Vector3 screenPosition;
    private Vector3 worldPosition;

    [Header("Interactable")]
    public Interactable currentInteractable;
    public Interactable selectedInteractable;
    public InteractableSlot currentSlot;
    private bool isInEditorMode;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        InteractableDetection();

        screenPosition = Mouse.current.position.value;
        OnMouseMovement.Invoke(screenPosition);

        if (isInEditorMode)
            return;

        Movement();

    }

    void Movement()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, Mathf.Infinity, groundLayerMask))
            worldPosition = hitData.point;

        Vector3 desiredDirection = worldPosition - transform.position;

        //transform.position = Vector3.Lerp(transform.position, worldPosition, mouseLerp * Time.deltaTime);

        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition(worldPosition, out hit, .3f, NavMesh.AllAreas);

        if (!isValid)
            return;

        if ((transform.position - hit.position).magnitude >= .02f)
            transform.position = Vector3.Lerp(transform.position, worldPosition, mouseLerp * Time.deltaTime);

        //Visual Rotation

        if ((worldPosition - transform.position).magnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDirection), rotationSpeed * Time.deltaTime);
    }

    void InteractableDetection()
    {
        if (currentInteractable != null)
            if (currentInteractable.selected)
                return;

        screenPosition = Mouse.current.position.value;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, Mathf.Infinity))
        {
            if (hitData.transform.GetComponent<Interactable>() != null)
            {
                Interactable rayInteractable = hitData.transform.GetComponent<Interactable>();

                if (currentInteractable != rayInteractable)
                {
                    if (currentInteractable != null)
                        currentInteractable.Highlight(false);

                    currentInteractable = rayInteractable;
                    currentInteractable.Highlight(true);

                    //Event
                    OnHoverInteractable.Invoke(true);
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.Highlight(false);
                currentInteractable = null;

                //Event
                OnHoverInteractable.Invoke(false);
            }
        }

    }

    public float MovementMagnitude()
    {
        return (worldPosition - transform.position).magnitude;
    }

    #region Input

    void OnFire(InputValue value)
    {

        if (currentInteractable != null)
            currentInteractable.ClickHandler();
        else
            HeroManager.instance.SetHeroDestination(worldPosition);

        OnMouseClick.Invoke(worldPosition);
    }

    public void OnEdit()
    {
        ToggleEditMode();
    }

    public void ToggleEditMode()
    {
        isInEditorMode = !isInEditorMode;

        if (isInEditorMode)
        {
            transform.DOComplete();
            transform.DORotate(new Vector3(0, 180, 0), .2f, RotateMode.Fast);
        }

        OnEditorMode.Invoke(isInEditorMode);
    }

    #endregion

}
