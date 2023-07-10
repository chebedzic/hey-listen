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

    [Header("Procedural Animation")]
    [SerializeField] private Rig headAimRig;
    [SerializeField] private Transform headAimTarget;

    void Start()
    {
        companionManager = GetComponentInParent<CompanionManager>();
        companionAnimator = GetComponentInChildren<Animator>();

        //Event Listening
        companionManager.OnMouseClick.AddListener(PlayClickParticle);
        companionManager.OnEditorMode.AddListener(SetHeadAim);
    }

    private void Update()
    {
        companionAnimator.SetFloat("magnitude", companionManager.MovementMagnitude());

        Vector3 mousePos = Mouse.current.position.value;
        mousePos.z = 5;
        Vector3 properScreenPosition = Camera.main.ScreenToWorldPoint(mousePos);
        headAimTarget.position = properScreenPosition + Camera.main.transform.forward;

    }


    void PlayClickParticle(Vector3 position)
    {
        if (clickParticle == null)
            return;

        clickParticle.transform.position = position + (Vector3.up * particleUpOffset);
        clickParticle.Play();
    }

    void SetHeadAim(bool state)
    {
        DOVirtual.Float(state ? 0 : 1, state ? 1 : 0, .2f,SetRigWeight);
        
    }

    void SetRigWeight(float weight)
    {
        headAimRig.weight = weight;
    }
}
