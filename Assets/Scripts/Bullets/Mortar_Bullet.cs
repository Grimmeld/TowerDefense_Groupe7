using UnityEngine;
using System.Collections;

public class Mortar_Bullet : MonoBehaviour
{
    [Header("Config de la courbe")]
    public float duration = 1.5f;
    public float height = 3f;

    private Vector3 StartPos;
    public Vector3 TargetPos;
    private float time;
    public GameObject ExplosionPrefab;
    public GameObject ImpactEffect;
    [Header("Cible du bullet")]
    [SerializeField] public Transform target;
    [Header("Tag de l'ennemi")]
    public string enemyTag = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 5);
        Invoke(nameof(UpdateTarget), 0);
        InvokeRepeating("UpdateTarget", 8f, 8f);
        StartPos = transform.position;
        if (target != null)
        {
            TargetPos = target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (target == null)
        {
            return;
        }*/
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        Vector3 pos = Vector3.Lerp(StartPos, TargetPos, t);
        pos.y += height * Mathf.Sin(Mathf.PI * t);
        transform.position = pos;

        if (t >= 1)
        {
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
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
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            TargetPos = target.position;
        }
        else
        {
            target = null;
        }
    }

}
