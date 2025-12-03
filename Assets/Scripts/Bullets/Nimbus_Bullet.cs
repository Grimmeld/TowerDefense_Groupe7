using UnityEngine;

public class Nimbus_Bullet : MonoBehaviour
{
    [Header("Stats du projectile")]
    public float speed;
    [Header("transform")]
    public Transform BulletTransform;
    public GameObject NimbusCloud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        BulletTransform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void OnDestroy()
    {
        Instantiate(NimbusCloud, transform.position, transform.rotation);
    }

}
