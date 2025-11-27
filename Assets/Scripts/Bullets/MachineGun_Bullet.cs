using UnityEngine;

public class MachineGun_Bullet : MonoBehaviour
{
    [Header("Stats")]
    public float Speed;
    [Header("Transform")]
    public Transform BulletTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletTransform.Translate(Speed * Time.deltaTime, 0, 0);
    }
}
