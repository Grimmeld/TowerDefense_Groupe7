using UnityEngine;
using UnityEngine.AI;

public class Enemy_Buggy : MonoBehaviour
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        Collide = GetComponent<BoxCollider>();
        Vector3 dest = Destination.transform.position;
        agent.destination = dest;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
