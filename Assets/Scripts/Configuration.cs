using UnityEngine;

[CreateAssetMenu(menuName = "Configuration")]
public class Configuration: ScriptableObject
{
    public int NumberOfBars;

    [Header("Single Frame Graph")]
    public bool IsSingleFrameGraphOn;

    [Header("Multi Frame Graph")]
    public bool IsMultiFrameGraphOn;
    public int HistoryFrameCount;
    public bool IsMirrored;
}