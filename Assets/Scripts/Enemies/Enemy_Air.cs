using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_Air : MonoBehaviour
{
    [Header("Stats")]

    public int Health;
    public int Speed;
    public int Worth;//Argent qu'il rapporte

    [Header("Destination")]

    public Transform Destination;

    [Header("Component")]

    public NavMeshAgent agent;
    public BoxCollider Collide;
    BulletType bulletType;
    WaveSpawner waveSpawner;

    //private Color OriginalColor;
    //public Color HitColor;
    //private Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rend = GetComponent<Renderer>();
        //OriginalColor = rend.material.color;
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        GameObject PointDest = GameObject.FindGameObjectWithTag("Destination");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        Collide = GetComponent<BoxCollider>();
        Vector3 dest = PointDest.transform.position;
        agent.destination = dest;
        waveSpawner.EnnemiesAlive++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        waveSpawner.EnnemiesAlive--;
        Destroy(gameObject);
    }

    void TakeDamage(int damage)
    {
        Health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletType bulletScript = other.GetComponent<BulletType>();
            TakeDamage(bulletScript.Damage);
            //StartCoroutine(HitFeedback());
            Destroy(other.gameObject);
        }


    }

    /*IEnumerator HitFeedback()
    {
        GetComponent<Renderer>().material.color = HitColor;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material.color = OriginalColor;
    }*/
}