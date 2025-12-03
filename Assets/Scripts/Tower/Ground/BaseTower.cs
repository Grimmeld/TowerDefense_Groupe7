using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum Turret_Type { Air, Ground}
public class BaseTower : MonoBehaviour
{
    TowerStats stats;
    TowerSabotaged towerSabotaged;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
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


    [Header("Stats")]
    private float ShootingRate;
    private float Shooting = 1f;
    private float RotationSpeed;
    private float Range;
    private float TurnSpeed = 1f;

    [SerializeField] public Transform target;
    public string enemyTag = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        ShootingRate = stats.shootingRate;
        Range = stats.Range;
        if (towerSabotaged.Sabotaged)
            return;
        if (type == Turret_Type.Air)
        {
            enemyTag = "EnemyAir";
        }
        if (type == Turret_Type.Ground)
        {
            enemyTag = "Enemy";
        }

        Shooting += Time.deltaTime;
        if(target == null)
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
