using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum Turret_Type { Air, Ground}
public class BaseTower : MonoBehaviour
{
    TowerStats stats;
    TowerSabotaged towerSabotaged;
    Tower_Animation tower_Animation;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
        towerSabotaged = GetComponent<TowerSabotaged>();
        tower_Animation = GetComponent<Tower_Animation>();
    }
    [Header("Type de la tourelle")]
    public Turret_Type type;

    [Header("Pivots")]
    public Transform TorsoPivot;
    public Transform ArmPivot;
    public Transform ShootingPoint;
    public Transform RobotHead;


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

    private bool waitingForAttackFire = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tower_Animation != null)
            tower_Animation.OnAttackFire -= HandleAttackFire;

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
            tower_Animation.State = Turret_State.Idling;
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
                tower_Animation.State = Turret_State.Attacking;
                Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
                Shooting = 0f;
            }
        }
        else
        {
            tower_Animation.State = Turret_State.Idling;
        }
    }

    private void HandleAttackFire()
    {
        Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
        Shooting = 0f;
        waitingForAttackFire = false;
    }
    private void LateUpdate()
    {
        if (target == null)
            return;
        Quaternion Offset = new Quaternion(0, 0, -180, 0);
        RobotHead.LookAt(target.transform.position);
        RobotHead.rotation = Quaternion.Slerp(RobotHead.rotation, RobotHead.rotation * Offset, TurnSpeed);
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
