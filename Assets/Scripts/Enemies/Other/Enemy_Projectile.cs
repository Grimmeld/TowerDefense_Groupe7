using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    [Header("Stats")]
    public float Speed;
    [Header("Transform")]
    public Transform BulletTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        BulletTransform.Translate(0, 0, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination"))
        {
            Destroy(gameObject);
        }
    }
}
