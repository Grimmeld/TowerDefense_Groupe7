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
        if (ResourceManager.instance != null
            && enemyStats != null)
        {
            ResourceManager.instance.AddGold(enemyStats.EnemyWorth);

            if (enemyStats.EnemyWorth == 0)
            {
                Debug.Log("Enemy have no worth");
            }
        }
    }

    public void TriggerDeath()
    {
        if (enemyAnimation != null)
        {
            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

           
            enemyAnimation.RefreshState();
            enemyAnimation.PlayDeath();

        }
        else
        {
            Destroy(gameObject);
        }
    }
}