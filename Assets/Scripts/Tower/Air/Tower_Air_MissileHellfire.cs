using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//public enum Turret_Type { Air, Ground }
public class Tower_Air_MissileHellfire : MonoBehaviour
{
    TowerStats stats;
    private void Awake()
    {
        stats = GetComponent<TowerStats>();
    }
    [Header("Type de la tourelle")]
    public Turret_Type type;

    [Header("Pivots")]
    public Transform TorsoPivot;
    public Transform ShootingPoint;


    [Header("Bullet")]
    public GameObject MissilePrefab;
    public int NumberOfMissileToShoot;
    private int MissilesShot;
    bool reloading = false;

    [Header("Stats")]
    private float ShootingRate = 1;
    private float Shooting = 1f;
    private float RotationSpeed;
    private float Range;
    private float TurnSpeed = 1f;
    private float ShootingReloadSpeed;

    [Header("Trajectoire des projectiles")]
    public float ConeSize;



    [SerializeField] public Transform target;
    public string enemyTag = "EnemyAir";

    public bool Sabotaged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MissilesShot = NumberOfMissileToShoot;//Nombre de missile a tirer dans la volée
        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame

    void Update()
    {
        Range = stats.Range;
        ShootingReloadSpeed = stats.shootingRate;
        if (Sabotaged)
            return;

        if (target == null)
        {
            return;
        }

        Shooting += Time.deltaTime;

        var TorsoLook = target.transform.position - TorsoPivot.position;
        TorsoLook.y = 0;
        var TorsoRotation = Quaternion.LookRotation(TorsoLook);

        if (Vector3.Distance(transform.position, target.transform.position) < Range)
        {
            TorsoPivot.rotation = Quaternion.Slerp(TorsoPivot.rotation, TorsoRotation, TurnSpeed);
            float xSpread = Random.Range(-1, 1);                                        //le spread des missile dans un cone
            float ySpread = Random.Range(-1, 1);
            Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * ConeSize;//Spread X taille du cone pour que ce soit modifiable
            Quaternion rotation = Quaternion.Euler(spread) * ShootingPoint.rotation;
            if (reloading)
                return;
            if (MissilesShot <= 0)//si la tourelle a tirer tout ses missiles, alors lancer la coroutine de rechargement
            {
                StartCoroutine(ReloadMissiles());
                return;
            }
            if (ShootingRate < Shooting)
            {
                Shooting = 0f;
                MissilesShot--;
                var missile = (GameObject)Instantiate(MissilePrefab, ShootingPoint.position, rotation);
            }
        }

        if (type == Turret_Type.Air)
        {
            enemyTag = "EnemyAir";
        }
        if (type == Turret_Type.Ground)
        {
            enemyTag = "Enemy";
        }
    }

    IEnumerator Sapped()
    {
        Sabotaged = true;
        yield return new WaitForSeconds(ShootingReloadSpeed);
        Sabotaged = false;
    }

    IEnumerator ReloadMissiles() //Recharger la volée de missile toute les 4 secondes
    {
        reloading = true;
        yield return new WaitForSeconds(4);
        MissilesShot = NumberOfMissileToShoot;
        reloading = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
    void UpdateTarget() //pour que le tourelle puisse regarde l'ennemi
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
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}
