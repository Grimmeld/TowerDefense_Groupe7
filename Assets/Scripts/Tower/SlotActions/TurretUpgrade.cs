using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretUpgrade : MonoBehaviour
{
    [SerializeField] private bool isUpgrading;

    [SerializeField] private GameObject currentTowerIG;
    [SerializeField] private GameObject currentTowerUpgrade;
    [SerializeField] private Transform spawnPoint;


    public void Upgrade(GameObject tower)
    {
        Debug.Log("Upgrade : " + tower.name);

        EnableIsUpgrading(true);
        currentTowerIG = tower;

        // Mettre le jeu en pause
        Time.timeScale = 0f;

        // Close panel HUD (ou le mettre en arrière) && open upgrade panel
        // & change camera view
        if (UpgradeMenu.Instance != null)
        {
            UpgradeMenu.Instance.SwitchPanel(this, isUpgrading);
        }

        // Faire apparaitre le mecha en question
        spawnPoint = UpgradeMenu.Instance.GetSpawnPoint();

        if (spawnPoint != null)
        {
            currentTowerUpgrade = Instantiate(tower, spawnPoint);
            currentTowerUpgrade.transform.position = spawnPoint.position; // remettre la position de la tour sur le spawnpoint
            Rigidbody r = currentTowerUpgrade.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.useGravity = false;
            }
        }
        else
        {
            Debug.Log("Spawn Point non trouvé");
        }

    }

    public void EnableIsUpgrading(bool enable)
    {
        isUpgrading = enable;
    }

    public void GetNewTower(GameObject tower)
    {
        Debug.Log("get new tower");
        // Creer les nouvelles tours avec les upgrade
        TurretSlot turretSlot = GetComponent<TurretSlot>();
        
        // Put the new tower in game on the slot
        GameObject go = Instantiate(tower, turretSlot.transform);
        go.transform.position = turretSlot.spawnPointTurret.position;
        Destroy(currentTowerIG);
        turretSlot.UpdateTurretSlot(go);

        // Voir la nouvelle dans le menu amelioration
        Destroy(currentTowerUpgrade);
        currentTowerUpgrade = Instantiate(tower, spawnPoint);

        // When closing menu, detruire la tour dans le menu

        // Fermer le menu d'amelioration
        isUpgrading = !isUpgrading;
        if (UpgradeMenu.Instance != null)
        {
            UpgradeMenu.Instance.CloseUpgradeMenu();
        }

    }

}
