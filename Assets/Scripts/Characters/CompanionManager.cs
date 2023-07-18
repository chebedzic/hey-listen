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
    [HideInInspector] public UnityEvent<Vector3> OnMouseMovement;
    [HideInInspector] public UnityEvent<Vector3> OnMouseClick;
    [HideInInspector] public UnityEvent<Action> OnActionCollect;
    [HideInInspector] public UnityEvent<bool> OnEditorMode;

    [Header("Parameters")]
    [SerializeField] private float mouseLerp = 10;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private LayerMask groundLayerMask;

    private Vector3 screenPosition;
    private Vector3 worldPosition;

    [Header("Hold Action")]
    public Action holdedAction;

    [Header("Interactable")]
    public Interactable currentInteractable;
    public InteractableSlot currentSlot;
    public InteractableModal currentModal;

    [Header("Edit Mode")]
    public bool isInEditorMode;

    public CombinationLibrary combinationLibrary;

    public void SetHoldedAction(Action action) 
    {
        holdedAction = action;

        OnActionCollect?.Invoke(action);
    }

    private void Awake()
    {
        instance = this;

    }

    private void Update()
    {
        screenPosition = Mouse.current.position.value;
        OnMouseMovement.Invoke(screenPosition);

        if (isInEditorMode)
            return;

        Movement();

        //Exception needed because Input System is currently not working on WEBGL
#if PLATFORM_WEBGL
        if(Mouse.current.leftButton.wasPressedThisFrame) OnFire();
#endif
    }

    void Movement()
    {
        if (currentModal != null)
        {
            Vector3 modalPosLerped = Vector3.Lerp(transform.position, currentModal.companionPositionReference.position, mouseLerp * Time.deltaTime);
            transform.position = modalPosLerped;
            transform.LookAt(Vector3.Lerp(transform.position, currentModal.transform.position, mouseLerp * Time.deltaTime));
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, Mathf.Infinity, groundLayerMask))
            worldPosition = hitData.point;

        Vector3 desiredDirection = worldPosition - transform.position;

        transform.position = Vector3.Lerp(transform.position, worldPosition, mouseLerp * Time.deltaTime);

        if ((worldPosition - transform.position).magnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDirection), rotationSpeed * Time.deltaTime);
    }

    public float MovementMagnitude()
    {
        return (worldPosition - transform.position).magnitude;
    }

    #region Input

    void OnFire()
    {

        if (currentInteractable == null && currentModal == null)
            if(!HeroManager.instance.isInteracting)
                HeroManager.instance.SetHeroDestination(worldPosition);

        OnMouseClick.Invoke(worldPosition);
    }

    void OnReset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

}
