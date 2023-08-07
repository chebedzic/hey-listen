using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSound : MonoBehaviour
{
    ParticleSystem ps;
    bool protect;
    [SerializeField] AudioClipContainer audioClip;
    private CinemachineImpulseSource impulseSource;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.isPlaying  && !protect)
        {
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound()
    {
        protect = true;
        AudioManager.instance.PlaySFX(audioClip, null);
        if(impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        yield return new WaitUntil(() => !ps.isPlaying);
        protect = false;
    }
}
