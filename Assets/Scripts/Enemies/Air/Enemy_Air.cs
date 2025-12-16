using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_Air : MonoBehaviour
{
    EnemyHealthBar healthBar;
    EnemyStats stats;
    EnemyModifier enemyModifier;
    EnemyAnimation enemyAnimation;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        enemyModifier = GetComponent<EnemyModifier>();
        healthBar = GetComponent<EnemyHealthBar>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }
    [Header("Stats")]

    public float Health;
    private float Speed;
    private float Worth;//Argent qu'il rapporte
    private float EnemyResistance;
    private float HealthRegen;
    [SerializeField] private float MaxHealth;


    [Header("Destination")]

    public Transform Destination;

    [Header("Component")]
    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;

    [Header("Death")]
    private EnemyDeath enemyDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        MaxHealth = Health;
        TargetManager.instance.RegisterEnemy(gameObject);
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        GameObject PointDest = GameObject.FindGameObjectWithTag("Destination");
        agent = GetComponent<NavMeshAgent>();
        Collide = GetComponent<BoxCollider>();
        Vector3 dest = PointDest.transform.position;
        agent.destination = dest;
        WaveSpawner.Instance.EnnemiesAlive++;
        WaveSpawner.Instance.Triger();
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
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        SetStats();
        //BEG LEA -- // Best if death is triggered only once, not each frame
        //if(Health <= 0)
        //{
        //    Death();
        //}
        // END LEA ++
    }
    private void OnTriggerEnter(Collider other)
    {
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
            enemyAnimation.PlayDeath();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
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
            agent.speed = Speed;
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
}