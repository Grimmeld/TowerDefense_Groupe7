using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_Air : MonoBehaviour
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


    [Header("Destination")]

    public Transform Destination;

    [Header("Component")]
    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        TargetManager.instance.RegisterEnemy(gameObject);
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        GameObject PointDest = GameObject.FindGameObjectWithTag("Destination");
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
        }


    }
    void Death()
    {
        WaveSpawner.Instance.OnEnemyDied();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}