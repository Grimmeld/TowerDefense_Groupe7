using UnityEngine;

public class BulletCollide : MonoBehaviour
{
    public string EnemyTag;
    public string EnemyAirTag;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(EnemyTag);
        Debug.Log(EnemyAirTag);

        //if (other.CompareTag(EnemyTag))
        if (other.gameObject.CompareTag(EnemyTag))
        {
            Destroy(gameObject);
        }
        //if (other.CompareTag(EnemyAirTag))
        if (other.gameObject.CompareTag(EnemyAirTag))
        {
            Destroy(gameObject);
        }
    }
}
