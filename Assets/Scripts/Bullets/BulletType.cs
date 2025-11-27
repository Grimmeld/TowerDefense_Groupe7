using UnityEngine;


public enum Bullet_Type { Mortar, Bullet, Missile }
public class BulletType : MonoBehaviour
{
    [Header("Type de la balle")]
    public Bullet_Type bulletType;

    [Header("Stats")]
    public int Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
