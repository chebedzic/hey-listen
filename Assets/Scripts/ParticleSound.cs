using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSound : MonoBehaviour
{
    ParticleSystem ps;
    bool protect;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
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
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Parry, null);
        yield return new WaitUntil(() => !ps.isPlaying);
        protect = false;
    }
}
