using UnityEngine;

[CreateAssetMenu(menuName = "Mesh Config")]
public class MeshConfig : ScriptableObject
{
    public int NumberOfBars;
    public int HistoryFrameCount;
    public float boostAmount = 10;
}