using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioSource[] sfxSources;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambientSource;
    public SoundSettings audioSettings;

    [SerializeField] private int sfxIndex = 0;

    private void Awake()
    {
        instance = this;
        float ambientVolume = ambientSource.volume;
        ambientSource.volume = 0;
        ambientSource.DOFade(ambientVolume, .5f);
    }

    public void SetAmbientVolume(float volume, float duration = .5f)
    {
        ambientSource.DOFade(volume, duration);
    }

   public void SetMusicVolume(float volume, float duration = .2f)
    {
        musicSource.DOFade(volume, duration);
    }

    public void PlaySFX(AudioClipContainer soundGroup, AudioSource source)
    {
        AudioSource tempSource = source == null ? sfxSources[sfxIndex] : source;

        int RandomIndex = Random.Range(0, soundGroup.clips.Length);

        float pitch = soundGroup.pitch;

        if (soundGroup.randomPitch)
            pitch = Random.Range(pitch - soundGroup.randomPitchVariation, pitch + soundGroup.randomPitchVariation);

        sfxSources[sfxIndex].pitch = pitch;
        tempSource.pitch = pitch;

        tempSource.PlayOneShot(soundGroup.clips[RandomIndex], soundGroup.volume);

        sfxIndex = (int)Mathf.Repeat(sfxIndex + 1, sfxSources.Length);
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

}
