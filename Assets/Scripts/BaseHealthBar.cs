using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealthBar : MonoBehaviour
{
    [Header("Values")]
    public float Health;
    public float MaxHealth;

    [Header("Layout (fallback)")]
    public float Width;
    public float Height;

    [Header("References")]
    [SerializeField] private RectTransform HealthBar;   
    [SerializeField] private Image HealthBarImage;      
    public TextMeshProUGUI currentHealthText;

    private void Awake()
    {
        if (HealthBarImage == null)
            HealthBarImage = GetComponentInChildren<Image>();

        if (HealthBarImage != null)
        {
            HealthBarImage.type = Image.Type.Filled;
            HealthBarImage.fillMethod = Image.FillMethod.Horizontal;
            HealthBarImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        if (HealthBar == null)
        {
            Transform t = transform.Find("HealthBar");
            if (t != null) HealthBar = t.GetComponent<RectTransform>();
            else if (HealthBarImage != null) HealthBar = HealthBarImage.rectTransform;
        }

        if (HealthBar != null)
        {
            if (Width <= 0f) Width = HealthBar.sizeDelta.x;
            if (Height <= 0f) Height = HealthBar.sizeDelta.y;
        }
    }

    private void Update()
    {
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
            Debug.LogWarning($"[{nameof(BaseHealthBar)}] MaxHealth is zero. Call SetMaxHealth first.");
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
            HealthBarImage.SetAllDirty();
        }
        else if (HealthBar != null)
        {
            float newWidth = Width * normalized;
            HealthBar.sizeDelta = new Vector2(newWidth, Height);
        }

        if (currentHealthText != null)
            currentHealthText.text = $"{Mathf.CeilToInt(Health)}/{Mathf.CeilToInt(MaxHealth)}";
    }
}