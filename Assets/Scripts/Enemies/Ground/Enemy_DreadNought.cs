using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_DreadNought : MonoBehaviour
{
    EnemyStats stats;
    EnemyModifier enemyModifier;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        enemyModifier = GetComponent<EnemyModifier>();
    }
    [Header("Stats")]

    private float Health;
    private float Speed;
    private float Worth;//Argent qu'il rapporte
    private float EnemyResistance;
    private float HealthRegen;
    [Header("DreadNought Drop Ennemi")]
    public int EnemiesToSpawn;
    public float EnemySpawnRate;
    public float EnemySpawn;

    [Header("Destination")]

    public Transform Destination;

    [Header("Component")]
    public GameObject EnemyBuggyPrefab;
    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;
    public Transform EnemySpawnPoint;

    [Header("Feedback")]
    private Color OriginalColor;
    public Color HitColor;
    private Renderer rend;

    [Header("Death")]
    private EnemyDeath enemyDeath;

    [Header("Waypoints")]
    public string WaypointTag = "Waypoint";
    public float ReachDistance = 0.3f;
    private Transform chosenWaypoint;
    private bool waypointReached;
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
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

        enemyDeath = GetComponent<EnemyDeath>();
        if (enemyDeath == null)
        {
            Debug.Log("No death script found");
        }
        PickOneOfThreeClosestWaypoints();
        SetStats();
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyResistance = stats.EnemyResistance;
        EnemySpawn += Time.deltaTime;
        SetStats();
        if (EnemySpawn > EnemySpawnRate)
        {
            StartCoroutine(SpawnEnemiesBuggy());
            EnemySpawn = 0;
        }

        if (!waypointReached && chosenWaypoint != null && agent != null)
        {
            // utilise un component du navmesh qui calcule la distance restante
            bool arrived = false;
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance + ReachDistance)
                    arrived = true;
                else if (agent.remainingDistance == Mathf.Infinity)
                {
                    if (Vector3.Distance(transform.position, chosenWaypoint.position) <= ReachDistance)
                        arrived = true;
                }
            }

            if (arrived)
            {
                waypointReached = true;
                //Aller vers la base
                if (Destination != null)
                {
                    agent.SetDestination(Destination.position);
                }
            }
        }

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
        damage = damage -= EnemyResistance;
        Health -= damage;

        // Check health when damage is done
        if (Health <= 0)
        {
            Death();
        }
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

    IEnumerator SpawnEnemiesBuggy()
    {
        int t = 0;
        while (t < 4)
        {
            Instantiate(EnemyBuggyPrefab, EnemySpawnPoint.position, EnemySpawnPoint.rotation);
            yield return new WaitForSeconds(0.5f);
            t++;
        }
        t = 0;
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
        }
        else
        {
            HealthRegen = 0;
        }


    }

    void PickOneOfThreeClosestWaypoints()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogWarning($"[{nameof(Enemy_Buggy)}] No NavMeshAgent found.");
                return;
            }
        }

        GameObject[] points = GameObject.FindGameObjectsWithTag(WaypointTag);
        if (points == null || points.Length == 0)
        {
            if (Destination != null) agent.SetDestination(Destination.position);
            return;
        }
        var threeClosest = points
            .OrderBy(p => Vector3.Distance(transform.position, p.transform.position))
            .Take(3)
            .ToArray();

        // prend un waypoint au hazard
        var chosen = threeClosest[Random.Range(0, threeClosest.Length)];
        chosenWaypoint = chosen.transform;
        waypointReached = false;

        agent.SetDestination(chosenWaypoint.position);
    }
}
