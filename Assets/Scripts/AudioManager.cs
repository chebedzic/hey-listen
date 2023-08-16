using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource[] sfxSources;
    [SerializeField] AudioSource baseMusicSource;
    [SerializeField] AudioSource[] musicLayers;
    [SerializeField] AudioSource ambientSource;
    public SoundSettings audioSettings;

    [SerializeField] private int sfxIndex = 0;

    private void Awake()
    {
        instance = this;
        float ambientVolume = ambientSource.volume;
        ambientSource.volume = 0;
        ambientSource.DOFade(ambientVolume, .5f);
        SetMusicMixerVolume(0);
    }

    public void SetAmbientVolume(float volume, float duration = .5f)
    {
        ambientSource.DOFade(volume, duration);
    }

   public void SetMusicVolume(float volume, float duration = .2f)
    {
        DOVirtual.Float(GetMusicLevel(), volume, duration, SetMusicMixerVolume);
    }

    public void SetMusicMixerVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }

    public float GetMusicLevel()
    {
        float value;
        bool result = audioMixer.GetFloat("music", out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }

    public void SetLayerVolume(int layer, float volume)
    {
        musicLayers[layer].DOFade(volume, .2f);
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

        tempSource.bypassEffects = soundGroup.bypassEffect;
        tempSource.bypassReverbZones = soundGroup.bypassEffect;

        tempSource.PlayOneShot(soundGroup.clips[RandomIndex], soundGroup.volume);

        sfxIndex = (int)Mathf.Repeat(sfxIndex + 1, sfxSources.Length);
    }

    public void PlayMusic()
    {
        baseMusicSource.Play();
        foreach (AudioSource layer in musicLayers)
        {
            layer.volume = 0;
            layer.Play();
        }
    }

}
