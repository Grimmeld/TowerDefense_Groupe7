using UnityEngine;

public class BulletCollide : MonoBehaviour
{
    public string EnemyTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Destroy(gameObject);
        }
    }
}
