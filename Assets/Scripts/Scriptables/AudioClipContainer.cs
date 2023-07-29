using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio Clip Container")]
public class AudioClipContainer : ScriptableObject
{
    public AudioClip[] clips;
    public float volume = 1;
    public float pitch = 1;
    public bool randomPitch = false;
    public float randomPitchVariation = .1f;
}
