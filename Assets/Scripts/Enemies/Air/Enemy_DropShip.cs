using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Waypoints")]
    public string WaypointTag = "Waypoint";
    public float ReachDistance = 0.3f;

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

    public Transform Destination;

    [Header("Death")]
    private EnemyDeath enemyDeath;

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
        enemyDeath = GetComponent<EnemyDeath>();
        if (Destination == null)
        {
            var destObj = GameObject.FindGameObjectWithTag("Destination");
            if (destObj != null) Destination = destObj.transform;
        }
        PickOneOfThreeClosestDrops();
        if (enemyDeath == null)
        {
            Debug.Log("No death script found");
        }
    }
    bool isDropping = false;
    bool Dropped = false;


    // Update is called once per frame
    void Update()
    {

        //BEG LEA -- // Best if death is triggered only once, not each frame
        //if(Health <= 0)
        //{
        //    Death();
        //}
        // END LEA ++

        if (isDropping) return;

        // if we have a chosen targetPosition (drop point), navigate to it
        if (targetPosition != null)
        {
            Vector3 dest = targetPosition.transform.position;
            agent.SetDestination(dest);

            if (Vector3.Distance(transform.position, dest) < 0.5f)
            {
                if (!Dropped)
                {
                    StartCoroutine(DeployEnemies());
                }
            }
        }
        else
        {
            // if no drop target found, go to the main Destination if available
            if (Destination != null)
            {
                agent.SetDestination(Destination.position);
            }
        }

        bool Grounded = Physics.Raycast(transform.position, Vector3.down, lineLength, LayerMask.GetMask("Ground"));
        Debug.DrawLine(transform.position, Vector3.down * 1.5f, Color.red);
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
        var destObj = GameObject.FindGameObjectWithTag("Destination");
        agent.SetDestination(destObj.transform.position);

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
        if (DestinationPoints == null || DestinationPoints.Count == 0) return;
        Transform dest2 = DestinationPoints[currentIndex % DestinationPoints.Count];
        agent.SetDestination(dest2.transform.position);
    }


    void PickOneOfThreeClosestDrops()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        GameObject[] drops = GameObject.FindGameObjectsWithTag(DropTag);
        if (drops == null || drops.Length == 0)
        {
            // nothing to drop on; go to main Destination immediately
            if (Destination != null)
            {
                agent.SetDestination(Destination.position);
            }
            return;
        }

        var threeClosest = drops
            .OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
            .Take(3)
            .ToArray();

        var chosen = threeClosest[Random.Range(0, threeClosest.Length)];
        targetPosition = chosen.transform;

        // set agent destination to chosen drop point
        if (agent != null && targetPosition != null)
        {
            agent.SetDestination(targetPosition.position);
        }
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
    public void TakeDamage(float damage)
    {
        Health -= damage;

        // Check health when damage is done
        if (Health <= 0)
        {
            Death();
        }
    }
}
