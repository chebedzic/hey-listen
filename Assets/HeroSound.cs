using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSound : MonoBehaviour
{
    private AudioSource audioSource;


    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    public void PlayClip(AudioGroup soundGroup)
    {
        int RandomIndex = Random.Range(0, soundGroup.clips.Length);

        float pitch = soundGroup.pitch;

        if (soundGroup.randomPitch)
            pitch = Random.Range(pitch - .2f, pitch + 2f);

        audioSource.pitch = pitch;

        audioSource.PlayOneShot(soundGroup.clips[RandomIndex], soundGroup.volume);
    }


    public void PlayStepSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_StepSound, audioSource);
    }

    public void PlayWhistleSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.companion_Whistle, audioSource);
    }

    public void PlayHurtSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_HurtSound, audioSource);
    }

    public void PlayEnemyHurtSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.enemy_Hurt, audioSource);
    }

    public void PlaySwordSound()
    {
        if(HeroManager.instance.currentEquipment != null)
        {
            if(HeroManager.instance.currentEquipment.type == EquipmentType.shield)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Attack, audioSource);
                return;
            }
        }
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Attack, audioSource);
    }

    public void PlayJumpSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Jump, audioSource);
    }

    public void PlaySpinSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Spin, audioSource);
    }
}
