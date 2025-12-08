using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [SerializeField] private CanvasRenderer hudPanel;
    [SerializeField] private CanvasRenderer uiUpgrade;

    [SerializeField] private Camera cameraUpgrade;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private TurretUpgrade upgrade;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public void SwitchPanel(TurretUpgrade tUpgrade, bool enable)
    {
        if (enable)
        {   // Open upgrade menu
            hudPanel.gameObject.SetActive(false);
            uiUpgrade.gameObject.SetActive(true);
            cameraUpgrade.depth = Camera.main.depth + 1;
            upgrade = tUpgrade;
        }
        else
        {   // Close upgrade menu
            hudPanel.gameObject.SetActive(true);
            uiUpgrade.gameObject.SetActive(false);
            cameraUpgrade.depth = Camera.main.depth - 1;

            upgrade.EnableIsUpgrading(false);
            upgrade = null;
        }

    }

    public Transform GetSpawnPoint()
        { return spawnPoint; }

    public void CloseUpgradeMenu()
    {
        SwitchPanel(upgrade, false);
        Time.timeScale = 1.0f;  
    }

    public void AirUpgrade(GameObject tower)
    {
        upgrade.GetNewTower(tower);
    }

    public void GroundUpgrade(GameObject tower)
    {
        upgrade.GetNewTower(tower);
    }

}
