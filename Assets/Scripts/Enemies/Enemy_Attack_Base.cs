using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack_Base : MonoBehaviour
{


    public MonoBehaviour EnnemyScript;
    EnemyStats stats;
    EnemyAnimation enemyAnimation;

    private float EnemyRange;
    private float FireRate;
    private bool AttackMode = false;
    private float Shooting;
    private bool attackTrigger = false;


    [Header("Projectile_Ennemi")]

    public Transform ShootingPoint;
    public GameObject EnemyProjectile;

    private void Awake()
    {
        EnnemyScript = GetComponent<MonoBehaviour>();
        stats = GetComponent<EnemyStats>();
        enemyAnimation = GetComponent<EnemyAnimation>();
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
        if (AttackMode)
        {
            if (Shooting > FireRate)
            {
                ShootingPoint.LookAt(Destination.transform.position);
                Instantiate(EnemyProjectile, ShootingPoint.position, ShootingPoint.rotation);
                Shooting = 0;
            }
        }
        if (Vector3.Distance(transform.position, Destination.transform.position) < EnemyRange)
        {
            agent.enabled = false;
            if (attackTrigger)
                return;
            enemyAnimation.PlayAnimation("Vehicle_To_Robot_Attack");
            attackTrigger = true;
            Invoke(nameof(PlayAttack),3);
            
        }


    }

    void PlayAttack()
    {
        AttackMode = true;
    }
}
