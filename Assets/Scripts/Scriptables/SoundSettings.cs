using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Settings")]
public class SoundSettings : ScriptableObject
{
    public AudioGroup hero_StepSound;
    public AudioGroup hero_HurtSound;
    public AudioGroup hero_Attack;
    public AudioGroup hero_Jump;
    public AudioGroup hero_Confusion;
    public AudioGroup hero_Clarity;
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
    public float randomPitchVariation = .1f;
}
