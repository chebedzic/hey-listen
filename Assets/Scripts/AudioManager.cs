using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioSource[] sfxSources;
    [SerializeField] AudioSource musicSource;
    public SoundSettings audioSettings;

    [SerializeField] private int sfxIndex = 0;

    private void Awake()
    {
        instance = this;
    }

   public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
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
