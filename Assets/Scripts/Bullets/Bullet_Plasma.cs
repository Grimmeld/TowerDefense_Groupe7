using UnityEngine;

public class Bullet_Plasma : MonoBehaviour
{
    public Plasma_Canon plasmaCanon;
    private void Awake()
    {
        plasmaCanon = GetComponent<Plasma_Canon>();
    }

    [Header("Stats")]
    public float Speed;
    [Header("Transform")]
    public Transform BulletTransform;
    [Header("Damage")]
    public float Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //Damage = plasmaCanon.TotalDamage;
        BulletTransform.Translate(Speed * Time.deltaTime, 0, 0);
    }
}
