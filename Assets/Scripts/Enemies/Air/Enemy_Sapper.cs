using NUnit;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.AI;

public class Enemy_Sapper : MonoBehaviour
{
    EnemyStats stats;
    EnemyModifier enemyModifier;
    TowerSabotaged towerSabotaged;
    NavMeshAgent NavMeshAgent;
    EnemyHealthBar healthBar;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        towerSabotaged = GetComponent<TowerSabotaged>();
        enemyModifier = GetComponent<EnemyModifier>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        healthBar = GetComponent<EnemyHealthBar>();
    }
    public float Health;
    private float Speed;
    private float Worth;
    private float EnemyResistance;
    private float HealthRegen;
    [SerializeField] private float MaxHealth;

    [SerializeField] public Transform targetPosition;
    public string TurretTag = "Turret";
    public Vector3 enemyPos;

    [Header("Death")]
    private EnemyDeath enemyDeath;

    [Header("Plunge settings")]
    [SerializeField] private float hoverHeight = 4f;
    [SerializeField] private float plungeHeight = 0.7f;
    [SerializeField] private float hoverBaseOffset = 2f;
    [SerializeField] private float plungeBaseOffset = 0.7f;
    [SerializeField] private float baseOffsetChangeSpeed = 6f;
    [SerializeField] private float heightChangeSpeed = 6f;
    [SerializeField] private float plungeStartDistance = 4f;
    [SerializeField] private float lookSlerpSpeed = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider boxcollider;
    bool isPlunging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxcollider = GetComponent<BoxCollider>();
        Debug.Log("Sapper est là");

        NavMeshAgent.baseOffset = 2f;
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        MaxHealth = Health;
        WaveSpawner.Instance.EnnemiesAlive++;
        WaveSpawner.Instance.Triger();
        FindTarget();
        if (targetPosition != null)
            FaceTargetImmediate(targetPosition);
        enemyDeath = GetComponent<EnemyDeath>();
        if (enemyDeath == null)
        {
            Debug.Log("No death script found");
        }
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health);
        }
        SetStats();
    }
    // Update is called once per frame
    void Update()
    {
        if (targetPosition == null)
            FindTarget();
        if(Vector3.Distance(transform.position, targetPosition.position) < 5f)
        {
            animator.SetTrigger("Attacking");
        }
        float dist = Vector3.Distance(transform.position, targetPosition.position);
        isPlunging = dist < plungeStartDistance;
        if (NavMeshAgent != null)
        {
            float targetOffset = isPlunging ? plungeBaseOffset : hoverBaseOffset;
            NavMeshAgent.baseOffset = Mathf.MoveTowards(NavMeshAgent.baseOffset, targetOffset, baseOffsetChangeSpeed * Time.deltaTime);
        }
        float desiredY = isPlunging ? plungeHeight : hoverHeight;
        Vector3 desiredPos = new Vector3(targetPosition.position.x, desiredY, targetPosition.position.z);
        transform.position = Vector3.MoveTowards(transform.position, desiredPos, Speed * Time.deltaTime);
        FaceTargetSmooth(targetPosition);
        if(isPlunging)
            NavMeshAgent.enabled = false;
    }
    public void FindTarget()
    {
        
        GameObject[] target = GameObject.FindGameObjectsWithTag(TurretTag);
        if (target.Length > 0)
        {
            GameObject destination = target[Random.Range(0, target.Length)];
            targetPosition = destination.transform;
        }
        else
        {
            Debug.Log("Le sapper n'a pas de cible");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TurretTag))
        {
            var TowerSabotaged = other.GetComponent<TowerSabotaged>();
            TowerSabotaged.Sapping();
            animator.SetTrigger("Locked");
            boxcollider.enabled = false;
            Invoke(nameof(Death), 8f);
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
        SetHealth(-damage);

        // Check health when damage is done
        if (Health <= 0)
        {
            Death();
        }
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        if (healthBar != null)
            healthBar.SetHealth(Health);
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
            healthBar.SetHealth(Health);
        }
        else
        {
            HealthRegen = 0;
        }
    }

    private void FaceTargetImmediate(Transform target)
    {
        if (target == null) return;
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude <= 0.0001f) return;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void FaceTargetSmooth(Transform target)
    {
        if (target == null) return;
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude <= 0.0001f) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lookSlerpSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        WaveSpawner.Instance.OnEnemyDied();
    }
}
