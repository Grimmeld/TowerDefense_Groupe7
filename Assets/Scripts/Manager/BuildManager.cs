using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public List<TurretSlot> turrets;



    private void Awake()
    {
        TurretChoiceCanvas = GameObject.Find("TurretBuildCanvas");
        TurretChoiceCanvas.SetActive(false);
        if(instance != null)
        {
            return;
        }
        instance = this;

        //if (ResourceManager == null)
        //{
        //    Debug.Log("Resource manager not set up in the Inspector");
        //    ResourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        //}
    }

    public bool BuildMode;
    public GameObject TurretChoiceCanvas;


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
    [Header("Tourelle à instancier")]
    public GameObject turretToBuild;


    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void EnableBuildMode()
    {
        // Check that the player has enough Nuclear resource to add a tower
        if(!ResourceManager.instance.CheckNuclear())
        {
            return;
        }

        BuildMode = true;
        EnableCanvas();
    }
    public void DisableBuildMode()
    {
        BuildMode = false;
    }

    /// <summary>
    /// TEMPORAIRE
    /// </summary>
    public void AirSelect()
    {
        turretToBuild = TurretMissilePrefab;
    }
    public void MachineGun()
    {
        turretToBuild = TurretMachineGunPrefab;
    }
    public void Mortar()
    {
        turretToBuild = TurretMortarPrefab;
    }
    public void EnableCanvas()
    {
        TurretChoiceCanvas.SetActive(true);
    }

    public void DisableCanvas()
    {
        TurretChoiceCanvas.SetActive(false);
    }
}
