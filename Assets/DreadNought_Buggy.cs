using NUnit;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DreadNought_Buggy : MonoBehaviour
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
        Health = stats.EnemyHealth;

        Worth = stats.EnemyWorth;

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

    [Header("Waypoints")]
    public string WaypointTag = "Waypoint";
    public float ReachDistance = 0.3f;

    [Header("Component")]

    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;

    private Color OriginalColor;
    public Color HitColor;
    private Renderer rend;

    [Header("Death")]
    private EnemyDeath enemyDeath;

    private Transform chosenWaypoint;
    private bool waypointReached;

    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        MaxHealth = Health;
        rend = GetComponent<Renderer>();
        OriginalColor = rend.material.color;

        if (Destination == null)
        {
            GameObject PointDest = GameObject.FindGameObjectWithTag("Destination");
            if (PointDest != null) Destination = PointDest.transform;
            else Debug.LogWarning($"[{nameof(Enemy_Buggy)}] No object with tag \"Destination\" found.");
        }

        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        agent = GetComponent<NavMeshAgent>();
        Collide = GetComponent<BoxCollider>();
        waveSpawner.EnnemiesAlive++;
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
        agent.SetDestination(Destination.position);
        SetStats();
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        SetStats();
        //regarder si on a atteind un waypoint

        //BEG LEA -- // Best if death is triggered only once, not each frame
        //if(Health <= 0)
        //{
        //    Death();
        //}
        // END LEA ++
    }

    void Death()
    {
        WaveSpawner.Instance.OnEnemyDied();
        Debug.Log("Enemy Buggy Death");

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletType bulletScript = other.GetComponent<BulletType>();
            TakeDamage(bulletScript.Damage);
            StartCoroutine(HitFeedback());
        }



    }


    IEnumerator HitFeedback()
    {
        GetComponent<Renderer>().material.color = HitColor;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material.color = OriginalColor;
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

    void SetDestination()
    {


        agent.SetDestination(chosenWaypoint.position);
    }
}
