using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    [Header("Effet Disponible")]
    public bool SpeedBuff;
    public bool ArmorBuff;
    public bool DamageBuff;
    public bool HealthRegen;
    public float effectValue;
    public int GoldBonus;

    public void ShowEffect(CardSO selectedCard)
    {

        if(selectedCard.effectType == CardEffect.SpeedIncrease)
        {
            SpeedBuff = true;
            effectValue = selectedCard.effectValue;
            GoldBonus = selectedCard.GoldBonus;
        }
        if (selectedCard.effectType == CardEffect.DamageIncrease)
        {
            DamageBuff = true;
            effectValue = selectedCard.effectValue;
            GoldBonus = selectedCard.GoldBonus;
        }
        if (selectedCard.effectType == CardEffect.ArmorIncrease)
        {
            ArmorBuff = true;
            effectValue = selectedCard.effectValue;
            GoldBonus = selectedCard.GoldBonus;
        }
        if (selectedCard.effectType == CardEffect.HealthRegen)
        {
            HealthRegen = true;
            effectValue = selectedCard.effectValue;
            GoldBonus = selectedCard.GoldBonus;
        }
    }

    public void DisableAllModifier()//désactiver tout les effets a la fin de la vague
    {
        SpeedBuff = false;
        ArmorBuff = false;
        DamageBuff = false;
        HealthRegen = false;
        effectValue = 0;
        GoldBonus = 0;
    }

    public float GetEffectValue()
    {
        return effectValue;
    }
    public int GetGoldBonus()
    {
        return GoldBonus;
    }
    private void Update()
    {

    }
}
