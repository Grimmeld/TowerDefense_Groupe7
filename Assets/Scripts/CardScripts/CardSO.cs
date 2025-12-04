using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public Sprite cardImage;
    public string cardText;

    public CardEffect effectType;

    public float effectValue;
    public bool isUnique;
    public int unlockLevel;
}

public enum CardEffect
{
    SpeedIncrease,
    ArmorIncrease,
    DamageIncrease,
    HealthRegen
        
}
