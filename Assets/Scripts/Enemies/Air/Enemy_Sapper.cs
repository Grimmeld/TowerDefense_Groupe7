using NUnit;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

public class Enemy_Sapper : MonoBehaviour
{
    EnemyStats stats;
    EnemyModifier enemyModifier;
    TowerSabotaged towerSabotaged;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        towerSabotaged = GetComponent<TowerSabotaged>();
        enemyModifier = GetComponent<EnemyModifier>();
    }
    private float Health;
    private float Speed;
    private float Worth;
    private float EnemyResistance;
    private float HealthRegen;

    [SerializeField] public Transform targetPosition;
    public string TurretTag = "Turret";
    public Vector3 enemyPos;

    [Header("Death")]
    private EnemyDeath enemyDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        FindTarget();

        enemyDeath = GetComponent<EnemyDeath>();
        if (enemyDeath == null)
        {
            Debug.Log("No death script found");
        }
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {

        //BEG LEA -- // Best if death is triggered only once, not each frame
        //if(Health <= 0)
        //{
        //    Death();
        //}
        // END LEA ++

        enemyPos = transform.position;
        Vector3 NewPos = new Vector3(enemyPos.x, 4, enemyPos.z);
        if (Vector3.Distance(transform.position, targetPosition.position) < 4f)
        {
            NewPos = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z);
            transform.LookAt(targetPosition.position);
        }
        if (targetPosition == null)
            FindTarget();
        Vector3 direction = Vector3.MoveTowards(NewPos, targetPosition.position, Speed * Time.deltaTime);
        transform.position = direction;
    }
    public void FindTarget()
    {
        
        GameObject[] target = GameObject.FindGameObjectsWithTag(TurretTag);
        GameObject destination = target[Random.Range(0, target.Length)];
        targetPosition = destination.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TurretTag))
        {
            var TowerSabotaged = other.GetComponent<TowerSabotaged>();
            TowerSabotaged.Sapping();
            Destroy(gameObject);
        }
        if (other.CompareTag("Bullet"))
        {
            BulletType bulletScript = other.GetComponent<BulletType>();
            TakeDamage(bulletScript.Damage);
        }
    }

    void Death()
    {
        WaveSpawner.Instance.OnEnemyDied();
        //Destroy(gameObject);

        // Determine when the enemy will die, if there is an animation or not
        if (enemyDeath != null)
        {
            enemyDeath.TriggerDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void TakeDamage(float damage)
    {
        damage -= EnemyResistance;
        Health -= damage;

        // Check health when damage is done
        if (Health <= 0)
        {
            Death();
        }
    }

    void SetStats()
    {
        if (EventManager.Instance.SpeedBuff)
        {
            Speed = stats.EnemySpeed + ((stats.EnemySpeed / 100) * (enemyModifier.EffectValue));
        }
        else
        {
            Speed = stats.EnemySpeed;
        }
        if (EventManager.Instance.ArmorBuff)
        {
            EnemyResistance = stats.EnemyResistance + ((stats.EnemyHealth / 100) * (enemyModifier.EffectValue));
        }
        else
        {
            EnemyResistance = stats.EnemyResistance;
        }
        if (EventManager.Instance.HealthRegen && Health < stats.EnemyHealth)
        {
            HealthRegen = enemyModifier.EffectValue;
            Health += HealthRegen * Time.deltaTime;
        }
        else
        {
            HealthRegen = 0;
        }
    }
}
