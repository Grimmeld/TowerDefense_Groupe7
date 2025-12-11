using UnityEngine;

public class Tower_UraniumGenerator : MonoBehaviour
{
    TowerStats towerStats;
    TowerSabotaged towerSabotaged;
    Tower_Animation tower_Animation;
    private void Awake()
    {
        towerStats = GetComponent<TowerStats>();
        towerSabotaged = GetComponent<TowerSabotaged>();
        tower_Animation = GetComponent<Tower_Animation>();
    }

    private int UraniumToGive;
    private bool sapped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UraniumToGive = towerStats.UraniumToGive;
        ResourceManager.instance.AddNuclear(UraniumToGive);
    }

    // Update is called once per frame
    void Update()
    {
        if (towerSabotaged.Sabotaged)
        {
            if(!sapped)
            {
                RemoveUranium();
                sapped = true;
            }
            return;
        }
        UraniumToGive = towerStats.UraniumToGive;
    }

    private void OnDestroy()
    {
        ResourceManager.instance.RemoveNuclear(UraniumToGive);
    }



    void RemoveUranium()
    {
            ResourceManager.instance.RemoveNuclear(UraniumToGive);
            Invoke(nameof(GiveBackRessources), towerSabotaged.SapDuration +1f);
    }

    void GiveBackRessources()
    {
        ResourceManager.instance.AddNuclear(UraniumToGive);
        sapped = false;
    }
}
