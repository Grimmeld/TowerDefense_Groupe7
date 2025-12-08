using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    EnemyStats enemyStats;
    BulletModifier bulletModifier;
    private void Awake()
    {
        bulletModifier = GetComponent<BulletModifier>();
        //enemyStats = GetComponentInParent<EnemyStats>();
        //Damage = enemyStats.EnemyDamage;
    }

    [Header("Stats")]
    public float Speed;
    public float Damage;
    [Header("Transform")]
    public Transform BulletTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        BulletTransform.Translate(0, 0, Speed * Time.deltaTime);
        if (EventManager.Instance.DamageBuff)
            Damage = bulletModifier.normalDamage * bulletModifier.EffectValue;
        else
            Damage = bulletModifier.normalDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination"))
        {
            Destroy(gameObject);
        }
    }
}
