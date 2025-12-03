using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_DreadNought : MonoBehaviour
{
    EnemyStats stats;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }
    [Header("Stats")]

    private float Health;
    private float Speed;
    private float Worth;//Argent qu'il rapporte
    private float EnemyResistance;
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

    private Color OriginalColor;
    public Color HitColor;
    private Renderer rend;
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        rend = GetComponent<Renderer>();
        OriginalColor = rend.material.color;
        GameObject PointDest = GameObject.FindGameObjectWithTag("Destination");
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        Collide = GetComponent<BoxCollider>();
        Vector3 dest = PointDest.transform.position;
        agent.destination = dest;
        waveSpawner.EnnemiesAlive++;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyResistance = stats.EnemyResistance;
        EnemySpawn += Time.deltaTime;

        if (EnemySpawn > EnemySpawnRate)
        {
            StartCoroutine(SpawnEnemiesBuggy());
            EnemySpawn = 0;
        }
        if (Health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        WaveSpawner.Instance.OnEnemyDied();
        Destroy(gameObject);
    }

    void TakeDamage(float damage)
    {
        damage = damage -= EnemyResistance;
        Health -= damage;
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
}
