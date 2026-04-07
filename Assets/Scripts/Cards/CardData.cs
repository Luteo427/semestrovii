using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/Cards/CardData")]
public class CardData : ScriptableObject
{
    
    [Header("Visual")]
    public Sprite FrontSprite;
    public Sprite BackSprite;

}
