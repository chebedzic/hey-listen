using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    public SoundSettings audioSettings;

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
        AudioSource tempSource = source == null ? sfxSource : source;

        int RandomIndex = Random.Range(0, soundGroup.clips.Length);

        float pitch = soundGroup.pitch;

        if (soundGroup.randomPitch)
            pitch = Random.Range(pitch - soundGroup.randomPitchVariation, pitch + soundGroup.randomPitchVariation);

        sfxSource.pitch = pitch;
        tempSource.pitch = pitch;

        tempSource.PlayOneShot(soundGroup.clips[RandomIndex], soundGroup.volume);
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

}
