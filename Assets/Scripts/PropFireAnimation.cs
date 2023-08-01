using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropFireAnimation : MonoBehaviour
{

    [SerializeField] private Light[] fireLights;
    [SerializeField] private ParticleSystem[] fireParticles;

    private float originalLightIntensity;

    [SerializeField] private bool litFromStart = true;

    private void Awake()
    {


        if (fireLights.Length <= 0)
            return;

        originalLightIntensity = fireLights[0].intensity;

        if (!litFromStart)
            SetFire(false);
    }

    public void SetFire(bool fire)
    {
        if (fireLights.Length <= 0)
            return;

        if (fire)
        {
            for (int i = 0; i < fireLights.Length; i++)
            {
                fireLights[i].intensity = 0;
                fireParticles[i].Play();

            }
        }
        else
        {
            for (int i = 0; i < fireLights.Length; i++)
            {
                fireParticles[i].Stop();
            }
        }

        for (int i = 0; i < fireLights.Length; i++)
        {
            fireLights[i].DOIntensity(fire ? originalLightIntensity : 0, .3f);

        }


    }
}
