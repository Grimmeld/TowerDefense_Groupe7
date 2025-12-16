using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_PlasmaCanon : MonoBehaviour
{
    TowerStats stats;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
        baseDamage = Damage;
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

    [Header("Damage Ramp")]
    public float damageRampPerSecond = 5f;
    public float maxDamageMultiplier = 3f;
    private int baseDamage;
    private float sustainedFireTime = 0f;
    private Transform lastTarget = null;

    [Header("Cible de la tourelle")]
    [SerializeField] public Transform target;
    public string enemyTag = "Enemy";

    [Header("Animation")]
    Tower_Animation tower_Animation;

    private void Start()
    {
        enemyTag = "Enemy";
        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        tower_Animation = GetComponent<Tower_Animation>();
    }

    private void Update()
    {

        Range = stats.Range;
        ShootingRate = stats.shootingRate;
        Shooting += Time.deltaTime;
        if (target == null)
        {
            sustainedFireTime = 0f;
            lastTarget = null;
            tower_Animation.State = Turret_State.Idling;
            return;
        }

        if (lastTarget != target)
        {
            sustainedFireTime = 0f;
            lastTarget = target;
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) < Range)
            {
                sustainedFireTime += Time.deltaTime;
            }
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
                Shoot();
                Shooting = 0;
            }
        }

    }
    public void Shoot()
    {
        float added = damageRampPerSecond * sustainedFireTime;
        float capped = Mathf.Min(baseDamage * maxDamageMultiplier, baseDamage + added);
        int currentDamage = Mathf.CeilToInt(capped);
        Damage = currentDamage;
        Vector3 dir = (target.position - ShootingPoint.position).normalized;
        Ray theRay = new Ray(ShootingPoint.position, transform.TransformDirection(dir * Range));
        Debug.DrawRay(ShootingPoint.position, transform.TransformDirection(dir * Range), Color.yellow);
        if (Physics.Raycast(theRay, out RaycastHit hit, Range))
        {
            Debug.Log("Raycast : " + hit.collider.name);
            Debug.DrawRay(transform.position, dir, Color.red);
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("tag hit");
                var enemyBuggy = hit.collider.GetComponent<Enemy_Buggy>();
                var enemyDread = hit.collider.GetComponent<Enemy_DreadNought>();
                if (enemyBuggy != null)
                {
                    Debug.Log("Take damage");
                    enemyBuggy.TakeDamage(Damage);
                }
                if (enemyDread != null)
                {
                    enemyDread.TakeDamage(Damage);
                }
                Debug.Log("Enemy touché - Damage: " + currentDamage);
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