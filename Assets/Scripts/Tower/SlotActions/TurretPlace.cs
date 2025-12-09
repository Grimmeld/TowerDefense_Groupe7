using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretPlace : MonoBehaviour
{
    public GameObject Place(GameObject tower, Transform spawnPoint)
    {
        // Pay the price of the tower
        if (ResourceManager.instance != null)
            ResourceManager.instance.UseNuclear();
        GameObject instantiatedTower = (GameObject)Instantiate(tower, transform);
        if (spawnPoint != null)
            instantiatedTower.transform.position = spawnPoint.position;

        Zone zone = GetComponentInParent<Zone>();

        if (zone != null)
        {
            TowerController towerController = instantiatedTower.GetComponent<TowerController>();
            if (towerController != null)
            {
                towerController.enabled = zone.isPowered;
            }
        }

        return instantiatedTower;


    }
}
