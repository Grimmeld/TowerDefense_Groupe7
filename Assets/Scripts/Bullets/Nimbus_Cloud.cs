using UnityEngine;

public class Nimbus_Cloud : MonoBehaviour
{
    public BulletType BulletType;
    public float HitRate;
    public float hit;

    private void Update()
    {
        hit += Time.deltaTime;
    }
    private void Awake()
    {
        BulletType = GetComponent<BulletType>();
    }
    private void Start()
    {
        Destroy(gameObject, 4);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("EnemyAir"))
        {
            if(hit > HitRate)
            {
                var enemyAir = other.GetComponent<Enemy_Air>();
                var enemyDrop = other.GetComponent<Enemy_DropShip>();
                var enemySap = other.GetComponent<Enemy_Sapper>();
                if (enemyAir != null)
                {
                    enemyAir.TakeDamage(BulletType.Damage);
                }
                if (enemyDrop != null)
                {
                    enemyDrop.TakeDamage(BulletType.Damage);
                }
                hit = 0;
            }

        }
    }
}
