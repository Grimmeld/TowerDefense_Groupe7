using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum Turret_Type { Air, Ground}
public class BaseTower : MonoBehaviour
{
    [Header("Type de la tourelle")]
    public Turret_Type type;

    [Header("Pivots")]
    public Transform TorsoPivot;
    public Transform ArmPivot;
    public Transform ShootingPoint;


    [Header("Bullet")]
    public GameObject BulletPrefab;


    [Header("Stats")]
    public float ShootingRate;
    public float Shooting = 1f;
    public float RotationSpeed;
    public float Range;
    public float TurnSpeed = 1f;

    [SerializeField] public Transform target;
    public string enemyTag = "Enemy";

    public bool Sabotaged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Sabotaged)
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

    IEnumerator Sapped()
    {
        Sabotaged = true;
        yield return new WaitForSeconds(4);
        Sabotaged = false;
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
