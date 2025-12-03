using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack_Base : MonoBehaviour
{
    public MonoBehaviour EnnemyScript;
    EnemyStats stats;

    private float EnemyRange;
    private float FireRate;
    private bool AttackMode = false;
    private float Shooting;


    [Header("Projectile_Ennemi")]

    public Transform ShootingPoint;
    public GameObject EnemyProjectile;

    private void Awake()
    {
        EnnemyScript = GetComponent<MonoBehaviour>();
        stats = GetComponent<EnemyStats>();
    }
    private NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRange = stats.EnemyRange;
        FireRate = stats.EnemyFireRate;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Shooting += Time.deltaTime;
        GameObject Destination = GameObject.FindGameObjectWithTag("Destination");
        if (Vector3.Distance(transform.position, Destination.transform.position) < EnemyRange)
        {
            agent.enabled = false;
            AttackMode = true;
        }

        if(AttackMode)
        {
            if(Shooting > FireRate)
            {
                ShootingPoint.LookAt(Destination.transform.position);
                Instantiate(EnemyProjectile, ShootingPoint.position, ShootingPoint.rotation);
                Shooting = 0;
            }
        }
    }
}
