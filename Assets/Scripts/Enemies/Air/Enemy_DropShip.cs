using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_DropShip : MonoBehaviour
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
    public float DeploySpeed;

    [Header("Destination")]

    public List<Transform> DestinationPoints;

    [Header("Component")]

    public NavMeshAgent agent;
    public BoxCollider Collide;

    private int currentIndex = 0;// point de navigation actuel
    public int EnemiesToDrop;

    public GameObject EnemyToSpawnPrefab;

    public Transform Ground;
    public Transform Destin;

    private Vector3 StartPos;
    private Vector3 EndPos;
    private bool OnGround;

    public float lineLength;

    public float AgentStartBaseOffset;

    private float time;
    public float duration = 5f;
    public string DropTag = "EnemyDrop";
    [SerializeField] public Transform targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = stats.EnemyHealth;
        Speed = stats.EnemySpeed;
        Worth = stats.EnemyWorth;
        AgentStartBaseOffset = 2f;
        agent.baseOffset = AgentStartBaseOffset;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        Collide = GetComponent<BoxCollider>();
        agent.SetDestination(DestinationPoints[currentIndex].position);
        FindTarget();
    }
    bool isDropping = false;
    bool Dropped = false;
    // Update is called once per frame
    void Update()
    {


        if (Health <= 0)
        {
            Death();
        }
        if (isDropping) return;
        Vector3 dest = targetPosition.transform.position;
        agent.SetDestination(dest);
        bool Grounded = Physics.Raycast(transform.position, Vector3.down, lineLength, LayerMask.GetMask("Ground"));
        Debug.DrawLine(transform.position, Vector3.down * 1.5f, Color.red);
        if (Vector3.Distance(transform.position, dest) < 0.5f)
        {
            if(!Dropped)
            {
                StartCoroutine(DeployEnemies());
            }
            else
            {
                SwitchPoint();
            }
        }
    }

    IEnumerator DeployEnemies()
    {
        isDropping = true;
        Dropped = true;
        agent.isStopped = true;


        StartPos = transform.position;
        EndPos = new Vector3(StartPos.x, 0.7f, StartPos.z);
        agent.baseOffset = 0;


        yield return StartCoroutine(MoveVertical(StartPos, EndPos));

        yield return new WaitForSeconds(2);
        int NumberToSpawn = 4;
        for (int i = 0; i < NumberToSpawn; i++)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(0.5f);
        }   


        yield return StartCoroutine(MoveVertical(EndPos, StartPos));
        agent.baseOffset = AgentStartBaseOffset;
        yield return new WaitForSeconds(1);

        
        currentIndex++;
        agent.isStopped = false;
        SwitchPoint();

    }

    void SpawnEnemies()
    {
        Instantiate(EnemyToSpawnPrefab, transform.position, transform.rotation);
        
    }
    IEnumerator MoveVertical(Vector3 from, Vector3 to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    void SwitchPoint()
    {
        Transform dest2 = DestinationPoints[currentIndex];
        agent.SetDestination(dest2.transform.position);
    }

    public void FindTarget()
    {

        GameObject[] target = GameObject.FindGameObjectsWithTag(DropTag);
        GameObject destination = target[Random.Range(0, target.Length)];
        targetPosition = destination.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletType bulletScript = other.GetComponent<BulletType>();
            TakeDamage(bulletScript.Damage);
            //StartCoroutine(HitFeedback());
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
