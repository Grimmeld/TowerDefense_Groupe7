using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower_Base : MonoBehaviour
{
    TowerStats towerStats;
    TowerSabotaged towerSabotaged;
    private void Awake()
    {
        towerStats = GetComponent<TowerStats>();
        towerSabotaged = GetComponent<TowerSabotaged>();
    }
    [Header("Type de la tourelle")]
    public Turret_Type type;

    [Header("Pivots")]
    public Transform TorsoPivot;
    public Transform ArmPivot;
    public Transform ShootingPoint;


    [Header("Bullet")]
    public GameObject BulletPrefab;


    private float ShootingRate;
    private float Shooting = 1f;
    private float RotationSpeed;
    private float Range;
    private float TurnSpeed = 1f;

    [Header("Cible de la tourelle")]
    [SerializeField] public Transform target;
    [SerializeField] public Transform Airtarget;
    public string enemyTag = "Enemy";
    public string enemyAirTag = "EnemyAir";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        Range = towerStats.Range;
        ShootingRate = towerStats.shootingRate;
        if (towerSabotaged.Sabotaged)
            return;

        Shooting += Time.deltaTime;
        if (target == null)
        {
            return;
        }


        var TorsoLook = target.transform.position - TorsoPivot.position;
        TorsoLook.y = 0;
        var TorsoRotation = Quaternion.LookRotation(TorsoLook);



        if (Vector3.Distance(transform.position, target.transform.position) < Range)
        {
            TorsoPivot.rotation = Quaternion.Slerp(TorsoPivot.rotation, TorsoRotation, TurnSpeed);
            ArmPivot.LookAt(target.transform.position);
            if (ShootingRate < Shooting)
            {
                Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
                Shooting = 0f;
            }
        }
    }
    void UpdateTarget()
    {
        GameObject[] Ennemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] EnnemiesA = GameObject.FindGameObjectsWithTag(enemyAirTag);
        float closestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject Enemy in Ennemies)
        {
            float distance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = Enemy;
            }

        }
        foreach (GameObject EnemyA in EnnemiesA)
        {
            float distance = Vector3.Distance(transform.position, EnemyA.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = EnemyA;
            }

        }
        if (nearestEnemy != null && closestDistance <= Range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
