using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Settings")]
public class SoundSettings : ScriptableObject
{
    public AudioClipContainer hero_StepSound;
    public AudioClipContainer hero_HurtSound;
    public AudioClipContainer hero_InteractSound;
    public AudioClipContainer hero_Attack;
    public AudioClipContainer hero_Equip;
    public AudioClipContainer hero_Parry;
    public AudioClipContainer hero_Spin;
    public AudioClipContainer hero_Jump;
    public AudioClipContainer hero_Confusion;
    public AudioClipContainer hero_Clarity;
    public AudioClipContainer enemy_Hurt;
    public AudioClipContainer boss_Hit;
    public AudioClipContainer interact_door;
    public AudioClipContainer close_door;
    public AudioClipContainer open_walls;
    public AudioClipContainer companion_Whistle;
    public AudioClipContainer UI_hover;
    public AudioClipContainer UI_releaseAction;
    public AudioClipContainer UI_cancel;

}