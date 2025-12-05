using UnityEngine;

public class EnemyModifier : MonoBehaviour
{
    EventManager eventManager;
    private void Awake()
    {
        eventManager = GetComponent<EventManager>();
    }
    [Header("Modifier")]
    public bool SpeedBuff;
    public bool ArmorBuff;
    public bool DamageBuff;
    public bool HealthRegen;
    [Header("PV Regen par seconde")]
    public float HealthRegenIncrease;

    [Header("Force du modifier (en  %)")]
    public float EffectValue;

    public void Update()
    {
        EffectValue = EventManager.Instance.effectValue;
    }
}
