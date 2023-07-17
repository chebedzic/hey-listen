using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Settings")]
public class SoundSettings : ScriptableObject
{
    public AudioGroup hero_StepSound;
    public AudioGroup boss_Hit;
    public AudioGroup interact_door;
    public AudioGroup companion_Whistle;
    public AudioGroup UI_hover;
    public AudioGroup UI_releaseAction;
    public AudioGroup UI_cancel;

}

[System.Serializable]
public class AudioGroup
{
    public AudioClip[] clips;
    public float volume = 1;
    public float pitch = 1;
    public bool randomPitch = false;
}
