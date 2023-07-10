using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Cursor Settings")]
public class CursorSettings : ScriptableObject
{
    public Sprite arrowCursor;
    public Color arrowColor;
    public Sprite handCursor;
    public Color handColor;
}
