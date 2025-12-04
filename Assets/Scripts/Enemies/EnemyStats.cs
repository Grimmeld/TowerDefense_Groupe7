using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    EnemyModifier enemyModifier;
    private void Awake()
    {
        enemyModifier = GetComponent<EnemyModifier>();
    }
    [Header("Stats")]
    public float EnemyHealth;
    public float EnemySpeed;
    //public float EnemyWorth; 
    public int EnemyWorth;
    public float EnemyResistance;
    public float EnemyRange;
    public float EnemyFireRate;
}
