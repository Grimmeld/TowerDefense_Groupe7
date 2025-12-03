using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class Missile_Bullet : MonoBehaviour
{
    [Header("Transform")]
    public Transform MissileTransform;
    [Header("Stats")]
    public float startSpeed;
    public float speed = 2f;
    public float startTime = 1f;
    [SerializeField] public Transform target;
    [Header("Tag de l'ennemi")]
    public string enemyTag = "EnemyAir";
    public Vector3 Offset;

    private bool isTargeting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartLaunching());
        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 8f, 8f);
        speed = 1f;
        Destroy(gameObject, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTargeting)
        {
            transform.Translate(0,0 , startSpeed * Time.deltaTime);
            return;
        }
        if (target == null)
            return;
        speed = 1.5f;
        Vector3 move = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target.position + Offset);
        transform.position = move;
    }

    IEnumerator StartLaunching()
    {
        yield return new WaitForSeconds(startTime);
        isTargeting = true;
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
