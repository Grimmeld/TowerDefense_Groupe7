using UnityEngine;

public class ShieldDeathSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var Enemy = other.GetComponent<EnemyStats>();
            if (Enemy != null)
            {
                Enemy.EnemyResistance = 2f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var Enemy = other.GetComponent<EnemyStats>();
            if (Enemy != null)
            {
                Enemy.EnemyResistance = 0f;
            }
        }
    }
}
