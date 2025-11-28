using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Prefab Des Tourelles
    /// </summary>
    [Header("Tourelle_MachineGun")]
    public GameObject TurretMachineGunPrefab;
    [Header("Tourelle_Mortar")]
    public GameObject TurretMortarPrefab;
    [Header("Tourelle_Missile")]
    public GameObject TurretMissilePrefab;
    [Header("Tourelle_Tesla")]
    public GameObject TurretTeslaPrefab;
    ///
    ///
    ///

    public GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
}
