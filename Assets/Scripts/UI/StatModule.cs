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
    [SerializeField] private TextMeshProUGUI description;

    public static StatModule Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        return;
    }

    public void SetInformation(Module module)
    {
        GameObject prefab = module.prefab;

        moduleName.text = module.moduleName;

        TowerInformation towerInfo = prefab.GetComponent<TowerInformation>();
        if (towerInfo != null)
        {
            typeModule.text = towerInfo.type.ToString();
            attackValue.text = towerInfo.damage.ToString();

        }



        levelNb.text = module.level.ToString();

        // Get attack value
        TowerStats towerStats = prefab.GetComponent<TowerStats>();

        rangeValue.text = towerStats.Range.ToString();
        fireRangeValue.text = towerStats.shootingRate.ToString();
    
        description.text = module.description.ToString();
    }

}
