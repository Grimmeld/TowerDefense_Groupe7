using UnityEngine;

public class Base : MonoBehaviour
{
    public float Health, MaxHealth;
    public BaseHealthBar healthBar;
    public Animator AttackedAnimator;

    void Start()
    {
        if (MaxHealth <= 0f)
        {
            Debug.LogWarning($"[{nameof(Base)}] MaxHealth not set on {gameObject.name}. Defaulting to 1.");
            MaxHealth = 1f;
        }

        // If inspector didn't set current Health, start full
        if (Health <= 0f) Health = MaxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health); // ensure bar matches current health immediately
        }
        else
        {
            Debug.LogWarning($"[{nameof(Base)}] healthBar reference is not assigned on {gameObject.name}.");
        }
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        if (healthBar != null)
            healthBar.SetHealth(Health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Enemy_Projectile projectile = other.GetComponent<Enemy_Projectile>();
            AttackedAnimator.SetTrigger("Attacked!");
            SetHealth(-projectile.Damage);
        }
    }
}