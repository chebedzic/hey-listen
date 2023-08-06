using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class CompanionVisual : MonoBehaviour
{

    private CompanionManager companionManager;
    private Animator companionAnimator;
    [SerializeField] ParticleSystem clickParticle;
    [SerializeField] float particleUpOffset;

    [Header("Holded Action Visual")]
    [SerializeField] Renderer holdedActionRenderer;
    [SerializeField] MeshFilter holdedActionMeshFilter;

    [Header("Procedural Animation")]
    [SerializeField] private Rig headAimRig;
    [SerializeField] private Transform headAimTarget;
    [SerializeField] private float aimDistanceToCamera = 6;

    [Header("Whistle Settings")]
    [SerializeField] private float shakeDuration = .2f;
    [SerializeField] private float shakeStrength = 1;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] ParticleSystem whistleParticle;

    private bool canWhistle = true;
    private HeroSound companionSound;

    void Start()
    {
        companionManager = GetComponentInParent<CompanionManager>();
        companionAnimator = GetComponentInChildren<Animator>();
        companionSound = GetComponentInChildren<HeroSound>();

        //Event Listening
        companionManager.OnMouseClick.AddListener(PlayClickParticle);
        companionManager.OnEditorMode.AddListener(SetHeadAim);
        companionManager.OnActionCollect.AddListener(ActionCollect);
    }

    private void Update()
    {
        //companionAnimator.SetFloat("magnitude", companionManager.MovementMagnitude());

        Vector3 mousePos = Mouse.current.position.value;
        mousePos.z = aimDistanceToCamera;
        Vector3 properScreenPosition = Camera.main.ScreenToWorldPoint(mousePos);
        headAimTarget.position = properScreenPosition + Camera.main.transform.forward;

    }

    private void ActionCollect(Action action)
    {
        holdedActionMeshFilter.gameObject.SetActive(action != null);

        if (action == null)
        {
            companionAnimator.SetTrigger("drop");
            return;
        }

        holdedActionMeshFilter.transform.DOComplete();
        holdedActionMeshFilter.transform.DOShakeScale(.2f, 1.5f, 20, 20, true);

        holdedActionRenderer.materials = new Material[] { holdedActionRenderer.materials[0], action.actionMaterial };

        holdedActionMeshFilter.mesh = action.actionMesh;

        companionAnimator.SetTrigger("collect");
    }

    void PlayClickParticle(Vector3 position)
    {
        //TODO replace with general state machine
        if (!canWhistle || (companionManager.currentInteractable != null && companionManager.currentInteractable.GetComponent<RoomBridge>() == null) || companionManager.currentModal != null)
            return;

        if (clickParticle == null)
            return;

        clickParticle.transform.position = position + (Vector3.up * particleUpOffset);
        clickParticle.Play();

        transform.DOComplete();
        transform.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, 90, true);

        whistleParticle.Play();
        companionAnimator.SetTrigger("whistle");

        companionSound.PlayWhistleSound();
    }

    void SetHeadAim(bool state)
    {
        canWhistle = !state;
        DOVirtual.Float(state ? 0 : 1, state ? 1 : 0, .2f,SetRigWeight);
        
    }

    void SetRigWeight(float weight)
    {
        headAimRig.weight = weight;
    }
}
