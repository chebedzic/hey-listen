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

    public void PlayClip(AudioClipContainer soundContainer)
    {
        int RandomIndex = Random.Range(0, soundContainer.clips.Length);

        float pitch = soundContainer.pitch;

        if (soundContainer.randomPitch)
            pitch = Random.Range(pitch - .2f, pitch + 2f);

        audioSource.pitch = pitch;

        audioSource.PlayOneShot(soundContainer.clips[RandomIndex], soundContainer.volume);
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

    public void PlayInteractSound()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_InteractSound, audioSource);
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
                AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.hero_Parry, audioSource);
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

    public void PlayOpenDoor()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.interact_door, audioSource);
    }

    public void PlayCloseDoor ()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioSettings.close_door, audioSource);


    }
}
