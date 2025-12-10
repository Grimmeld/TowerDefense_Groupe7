using TMPro;
using UnityEngine;

public class StatModule : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moduleName;
    [SerializeField] private TextMeshProUGUI typeModule;
    [SerializeField] private TextMeshProUGUI levelNb;
    [SerializeField] private TextMeshProUGUI attackValue;
    [SerializeField] private TextMeshProUGUI rangeValue;
    [SerializeField] private TextMeshProUGUI fireRangeValue;

    public static StatModule Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        return;
    }

    public void SetInformation(Module module)
    {
        moduleName.text = module.moduleName;
        typeModule.text = module.type.ToString();
        levelNb.text = module.level.ToString();

        // Get attack value
        GameObject prefab = module.prefab;
        TowerStats towerStats = prefab.GetComponent<TowerStats>();

        rangeValue.text = towerStats.Range.ToString();
        fireRangeValue.text = towerStats.shootingRate.ToString();
    
        TowerController towerController = prefab.GetComponent<TowerController>();

    }

}
