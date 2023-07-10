using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionVisual : MonoBehaviour
{

    private CompanionManager companionManager;
    private Animator companionAnimator;
    [SerializeField] ParticleSystem clickParticle;
    [SerializeField] float particleUpOffset;

    void Start()
    {
        companionManager = GetComponentInParent<CompanionManager>();
        companionAnimator = GetComponentInChildren<Animator>();

        //Event Listening
        companionManager.OnMouseClick.AddListener(PlayClickParticle);
    }

    private void Update()
    {
        //print(companionManager.MovementMagnitude());
        companionAnimator.SetFloat("magnitude", companionManager.MovementMagnitude());
    }

    void PlayClickParticle(Vector3 position)
    {
        if (clickParticle == null)
            return;

        clickParticle.transform.position = position + (Vector3.up * particleUpOffset);
        clickParticle.Play();
    }
}
