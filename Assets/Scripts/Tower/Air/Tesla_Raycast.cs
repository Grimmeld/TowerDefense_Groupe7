using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tesla_RaycastM : MonoBehaviour
{
    TowerStats stats;
    Tower_Animation towerAnimation;
    TowerSabotaged towerSabotaged;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
        towerAnimation = GetComponent<Tower_Animation>();
        towerSabotaged = GetComponent<TowerSabotaged>();
    }
    [Header("Type de la tourelle")]
    public Turret_Type type;

    [Header("Pivots")]
    public Transform TorsoPivot;
    public Transform ArmPivot;
    public Transform ShootingPoint;


    [Header("Bullet")]
    public LineRenderer lightningFX;


    [Header("Stats")]
    private float ShootingRate;
    private float Shooting = 1f;
    private float RotationSpeed;
    private float Range;
    private float TurnSpeed = 1f;
    public int Damage;

    [Header("Cible de la tourelle")]
    [SerializeField] public Transform target;
    public string enemyTag = "EnemyAir";


    private void Start()
    {
        enemyTag = "EnemyAir";
        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if(towerSabotaged.Sabotaged)
            return;
        Range = stats.Range;
        ShootingRate = stats.shootingRate;
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
                towerAnimation.State = Turret_State.Attacking;
                Shoot();
                Shooting = 0;
            }
        }
        else
        {
                       towerAnimation.State = Turret_State.Idling;
        }
    }
    public void Shoot()
    {
        
        Vector3 dir = (target.position - ShootingPoint.position).normalized;
        Ray theRay = new Ray(ShootingPoint.position, transform.TransformDirection(dir * Range));
        Debug.DrawRay(ShootingPoint.position, transform.TransformDirection(dir * Range), Color.yellow);
        if (Physics.Raycast(theRay, out RaycastHit hit, Range))
        {
            if (hit.collider.tag == "EnemyAir")
            {
                var enemyAir = hit.collider.GetComponent<Enemy_Air>();
                var enemyDrop = hit.collider.GetComponent<Enemy_DropShip>();
                var enemySap = hit.collider.GetComponent<Enemy_Sapper>();
                if (enemyAir != null)
                {
                    enemyAir.TakeDamage(Damage);
                }
                if (enemyDrop != null)
                {
                    enemyDrop.TakeDamage(Damage);
                }
                Debug.Log("Enemy touché");
            }
            StartCoroutine(PlayLightningFX(target));
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

    IEnumerator PlayLightningFX(Transform target)
    {
        lightningFX.enabled = true;
        lightningFX.SetPosition(0, ShootingPoint.position);
        lightningFX.SetPosition(1, target.position);
        yield return new WaitForSeconds(1);
        lightningFX.enabled = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}




