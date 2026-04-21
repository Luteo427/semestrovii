using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Coins")]
    public int CoinsGlobal = 15;
    public int CoinsLocal = 5;
    [Header("Organs")]
    public bool IsHearted = true;
    public bool IsBrained = true;
    public bool IsEyed = true;
    [Header("Scores")]
    public int MaxScore;
    public int LastScore;
    public int FinalScore;
    public int FirstScore;
    public int SecondScore;
    public int ThirdScore;
    
    
}
