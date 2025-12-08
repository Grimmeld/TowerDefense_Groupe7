using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Module;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [SerializeField] private CanvasRenderer hudPanel;
    [SerializeField] private CanvasRenderer uiUpgrade;

    [SerializeField] private Camera cameraUpgrade;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private TurretUpgrade upgrade;

    [SerializeField] private GameObject panelWeapon;

    [SerializeField] private List<Module> modules;

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

    public void SelectingModule(Module module)
    {
        Debug.Log("appuyer sur le module");
        SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();

        // Prendre le module
        if (module.type == typeModule.weapon)
        {
            
            for (int i = 0; i < slotImages.Length; i++)
            {
                // Si le slot est déjà plein, passer au suivant
                if (slotImages[i].gameObject.GetComponent<Image>().enabled)
                    continue;

                // Changer l'image sur l'affichage du menu
                slotImages[i].gameObject.GetComponent<Image>().enabled = true;
                slotImages[i].gameObject.GetComponent<Image>().sprite = module.sprite;

                Debug.Log(slotImages[i].name);

                break; // Quand un slot est vide, terminé la boucle
            }
        }
        else
        {

        }

        // Mettre à jour l'amelioration sur la mode
        upgrade.UpdateTowerInMenu(module.prefab);


        // Update affichage menu
        // Mettre les images et les stat Module des modules dans les emplacements tour ou stat


        // Est-ce que c'est une amelio de niveau 1, 2? 

        // (est-ce qu'on peut mettre l'amelio ?




        // Regarder si on a assez de gold

        // Si cancel : erase modules du menu

    }

    public void SaveInventory()
    {
        // Si sauvegarde : Save dans Upgrades de la tour gameObject
        Debug.Log("on sauvegarde");

        // Creer les nouvelles tours avec les upgrade
        upgrade.CreateNewTower();




        // Deslectionné tous les slot
        SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].gameObject.GetComponent<Image>().enabled = false;
            Debug.Log("on remets les images du menu à zero");
        }
    }

    private void FillInventory()
    {
        modules = upgrade.GetComponentInChildren<Upgrade>().GetModules();


        foreach (Module mod in modules)
        {
            if (mod.type == typeModule.weapon)
            {
                // Weapon modules 



            }
            else if (mod.type == typeModule.stat)
            {
                // Stat modules


            }
        }
    }



}
