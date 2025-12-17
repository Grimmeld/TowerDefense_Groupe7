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
            MaxHealth = 1f;
        }

        // If inspector didn't set current Health, start full
        if (Health <= 0f) Health = MaxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health); // ensure bar matches current health immediately
        }
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        if (healthBar != null)
            healthBar.SetHealth(Health);

        // Base destroyed
        if (Health <= 0)
        {
            if (VictoryManager.instance != null)
            {
                VictoryManager.instance.Defeat();
            }
        }

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