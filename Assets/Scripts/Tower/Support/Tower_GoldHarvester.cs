using UnityEngine;

public class Tower_GoldHarvester : MonoBehaviour
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
    private float ShootingRate;
    private float Shooting = 1f;
    private float goldPerShoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootingRate = towerStats.shootingRate;
        goldPerShoot = towerStats.goldToGive;
        if (WaveSpawner.Instance.buildMode == false && !towerSabotaged.Sabotaged)
        {
            Shooting += Time.deltaTime;
            if (ShootingRate < Shooting)
            {
                ResourceManager.instance.AddGold(10);
                Shooting = 0f;
            }
        }
        else if (WaveSpawner.Instance.buildMode == true)
        {
            Shooting = 0;
        }
    }
}
