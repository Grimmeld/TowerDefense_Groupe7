using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy_Buggy enemy_Buggy;
    private EnemyStats enemy_Stats;

    [Header("Values")]
    public float Health;
    public float MaxHealth;

    [SerializeField] private Image HealthBarImage;

    private void Awake()
    {
        enemy_Buggy = GetComponent<Enemy_Buggy>();
        enemy_Stats = GetComponent<EnemyStats>();
        Health = enemy_Buggy.Health;
        MaxHealth = enemy_Stats.EnemyHealth;
        if (HealthBarImage == null)
            HealthBarImage = GetComponentInChildren<Image>();

        if (HealthBarImage != null)
        {
            HealthBarImage.type = Image.Type.Filled;
            HealthBarImage.fillMethod = Image.FillMethod.Horizontal;
            HealthBarImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        }

        //if (MaxHealth <= 0f) MaxHealth = Mathf.Max(1f, MaxHealth); // avoid division by zero

        UpdateVisuals();
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
        if (Health <= 0f) Health = MaxHealth;
        UpdateVisuals();
    }
    public void SetHealth(float health)
    {
        if (MaxHealth <= 0f)
        {
            Debug.LogWarning($"[{nameof(EnemyHealthBar)}] MaxHealth is zero. Call SetMaxHealth first.");
            MaxHealth = Mathf.Max(1f, MaxHealth);
        }

        Health = Mathf.Clamp(health, 0f, MaxHealth);
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        float normalized = (MaxHealth <= 0f) ? 0f : Mathf.Clamp01(Health / MaxHealth);

        if (HealthBarImage != null)
        {
            HealthBarImage.fillAmount = normalized;
        }
    }
    void Update()
    {

    }
}