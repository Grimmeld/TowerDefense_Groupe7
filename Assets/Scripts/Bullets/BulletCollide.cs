using UnityEngine;

public class BulletCollide : MonoBehaviour
{
    public string EnemyTag;
    public string EnemyAirTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag(EnemyAirTag))
        {
            Destroy(gameObject);
        }
    }
}
