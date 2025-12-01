using UnityEngine;
using System.Collections;

public class Missile_Hellfire_Bullet : MonoBehaviour
{
    [SerializeField] public Transform target;
    [Header("Stats")]
    public float startSpeed;
    public float speed = 2f;
    public float startTime = 1f;
    public Vector3 Offset;

    private bool isTargeting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = TargetManager.instance.GetFreeTarget(); //fetch une cible parmis celle qui sont libre
        StartCoroutine(StartLaunching());
        speed = 1f;
        Destroy(gameObject, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            Destroy(gameObject);
        if (!isTargeting)
        {
            transform.Translate(0, 0, startSpeed * Time.deltaTime);
            return;
        }
        speed = 1.5f;
        Vector3 move = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target.position + Offset);
        transform.position = move;
    }

    private void OnDestroy() //si le missile explose, il sort sa cible de la liste de cible target
    {
        if (target != null)
        {
            TargetManager.instance.ReleaseTarget(gameObject);
        }
    }

    IEnumerator StartLaunching()
    {
        yield return new WaitForSeconds(startTime);
        isTargeting = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir"))
        {
            Destroy(gameObject);
        }
    }
}
