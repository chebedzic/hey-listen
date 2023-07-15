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
    public ModalScript currentModal;

    [Header("Edit Mode")]
    public bool isInEditorMode;

    public CombinationLibrary combinationLibrary;

    private void Awake()
    {
        instance = this;

        LightProbes.needsRetetrahedralization += LightProbes_needsRetetrahedralization;
    }

    private void LightProbes_needsRetetrahedralization()
    {
        LightProbes.Tetrahedralize();
    }

    private void Update()
    {
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

        //if (!isValid)
        //    return;

        if ((transform.position - hit.position).magnitude >= .02f)
            transform.position = Vector3.Lerp(transform.position, worldPosition, mouseLerp * Time.deltaTime);

        //Visual Rotation

        if ((worldPosition - transform.position).magnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDirection), rotationSpeed * Time.deltaTime);
    }

    public float MovementMagnitude()
    {
        return (worldPosition - transform.position).magnitude;
    }

    #region Input

    void OnFire(InputValue value)
    {

        if (currentInteractable == null && currentModal == null)
            HeroManager.instance.SetHeroDestination(worldPosition);

        OnMouseClick.Invoke(worldPosition);
    }

    public void OnEdit()
    {
        SetEditMode(!isInEditorMode);
    }

    void OnReset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void SetEditMode(bool state)
    {
        isInEditorMode = state;

        if (isInEditorMode)
        {
            transform.DOComplete();
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 finalPos = cameraPos + new Vector3(-3, -5, 2);
            transform.DOMove(finalPos, .3f, false);
            transform.DORotate(new Vector3(50, 15, 0), .3f, RotateMode.Fast);
        }

        OnEditorMode.Invoke(isInEditorMode);
    }

    #endregion

}
