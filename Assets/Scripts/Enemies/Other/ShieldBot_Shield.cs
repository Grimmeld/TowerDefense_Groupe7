using UnityEngine;

public class ShieldBot_Shield : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
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
