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
    public bool apply = false;

    [Header("Force du modifier (en  x)")]
    public float EffectValue;
    // Update is called once per frame
    void Update()
    {
        EffectValue = EventManager.Instance.effectValue;

        SetStats();
    }

    void SetStats()
    {

        if (EventManager.Instance.DamageBuff)
        {
            if(!apply)
            {
                normalDamage = normalDamage * EffectValue;
                apply = true;
            }

        }
        else
        {
            normalDamage = 3;
            apply = false;
        }
    }
}
