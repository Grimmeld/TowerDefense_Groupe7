using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private EnemyStats enemyStats;
    private EnemyAnimation enemyAnimation;
    private Enemy_Attack_Base enemyAttack;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyAttack = GetComponent<Enemy_Attack_Base>();
    }

    private void OnDestroy()
    {
        if(ResourceManager.instance != null
            && enemyStats != null)
        {
            // Add gold to player's resources
            ResourceManager.instance.AddGold(enemyStats.EnemyWorth);
            
            if (enemyStats.EnemyWorth == 0)
            {
                Debug.Log("Enemy doesn't have reward : Check Enemy Stat");
            }
        }
    }

    public void TriggerDeath()
    {

        if (enemyAnimation != null)
        {
            enemyAnimation.CheckDeath(true); // Trigger animation and destroy gameObject

            // Enemy should not be a target anymore
            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Two animations depends on the form of the enemy
            if (enemyAttack == null)
            {   // Attack enemy isn't found
                enemyAnimation.PlayAnimation("Death_Vehicule");
            }
            else
            {
                if (enemyAttack.enabled)
                {
                    enemyAnimation.PlayAnimation("Death_Attack");
                }
                else
                {
                    enemyAnimation.PlayAnimation("Death_Vehicule");
                }
            }
           

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
