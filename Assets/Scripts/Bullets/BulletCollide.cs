using UnityEngine;

public class BulletCollide : MonoBehaviour
{
    public string EnemyTag;
    public string EnemyAirTag;

    private void OnTriggerEnter(Collider other)
    {
        
        //if (other.CompareTag(EnemyTag))
        if (EnemyTag != null)
        { 
            if (other.gameObject.CompareTag(EnemyTag))
            {
                Destroy(gameObject);
            }

        }
        //if (other.CompareTag(EnemyAirTag))

        if (EnemyAirTag != null)
        {
            if (other.gameObject.CompareTag(EnemyAirTag))
            {
                Destroy(gameObject);
            }
        }
    }
}
