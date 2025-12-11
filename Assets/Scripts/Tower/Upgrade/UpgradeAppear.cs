using UnityEngine;

public class UpgradeAppear : MonoBehaviour
{
    [SerializeField] private GameObject towerIdle;
    // + Spawn point for each tower if needed

    public GameObject GetTowerIdle()
        { return towerIdle; }
}
