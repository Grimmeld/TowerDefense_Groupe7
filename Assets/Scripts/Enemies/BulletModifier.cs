using UnityEngine;

public class BulletModifier : MonoBehaviour
{
    EventManager eventManager;

    private void Awake()
    {
        eventManager = GetComponent<EventManager>();
    }
    [Header("Damage_Modifier")]
    public float normalDamage;
    public bool DamageBuff;

    [Header("Force du modifier (en  x)")]
    public float EffectValue;
    // Update is called once per frame
    void Update()
    {
        EffectValue = EventManager.Instance.effectValue;
    }
}
