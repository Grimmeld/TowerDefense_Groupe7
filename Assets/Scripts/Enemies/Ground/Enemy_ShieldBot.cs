using UnityEngine;
using UnityEngine.AI;

public class Enemy_ShieldBot : MonoBehaviour
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

    [Header("Destination")]

    public Transform Destination;

    [Header("Component")]

    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;

    private Color OriginalColor;
    public Color HitColor;
    private Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        }


    }
}
