using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Module;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [Header("Canvas panels")]
    [SerializeField] private CanvasRenderer hudPanel;
    [SerializeField] private CanvasRenderer uiUpgrade;

    [Header("Upgrade scene")]
    [SerializeField] private Camera cameraUpgrade;
    [SerializeField] private Transform spawnPoint;

    [Header("Which tower is upgrading")]
    [SerializeField] private TurretUpgrade upgrade;
    [SerializeField] private int goldUpgrades;

    [Header("Field in Canvas")]
    [SerializeField] private GameObject panelWeapon;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI goldUpgText;
    [SerializeField] private GameObject panelStatModule;
    

    [Header("Animation")]
    [SerializeField] private Animator animatorPanel;
    [SerializeField] private Button buttonWeapon;

    [Header("Parameters")]
    [SerializeField] private Upgrade[] upgrades;
    [SerializeField] private List<Module> modulesToAdd;

    [Header("Base")]
    [SerializeField] private Canvas baseCanvas;

    [Header("Music")]
    [SerializeField] private string clickMenu;
    [SerializeField] private string wrong;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Transform nuclearBase = GameObject.FindWithTag("Destination").GetComponent<Transform>();
        baseCanvas = nuclearBase.GetComponentInChildren<Canvas>();
    }

    public void SwitchPanel(TurretUpgrade tUpgrade, bool enable)
    {
        if (baseCanvas != null)
        {   // Show/Hide health bar base
            baseCanvas.gameObject.SetActive(!enable);
        }

        if (enable)
        {   // Open upgrade menu
            hudPanel.gameObject.SetActive(false);
            uiUpgrade.gameObject.SetActive(true);
            cameraUpgrade.depth = Camera.main.depth + 1;
            upgrade = tUpgrade;

            if (ResourceManager.instance != null)
            {
                goldText.text = ResourceManager.instance.CheckGold().ToString();
                goldUpgText.text = "";
            }

            panelStatModule.SetActive(false);

            buttonWeapon.onClick.Invoke();

            OpenInventory();




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

    //public void AirUpgrade(GameObject tower)
    //{
    //    upgrade.GetNewTower(tower);
    //}

    //public void GroundUpgrade(GameObject tower)
    //{
    //    upgrade.GetNewTower(tower);
    //}

    public void SelectingModule(Module module)
    {
        // Est-ce qu'on peut ajouter le module ? 
        if (ResourceManager.instance == null)
            return;


        // Est-ce que l'amelio est disponible ? (Bon level?)
        
        // Recup les upgrade de la mode
        GameObject towerInGame = upgrade.GetTowerInMenu();

        Upgrade[] upgrades = towerInGame.GetComponentsInChildren<Upgrade>();

        int maxLevel = 0;
        string nextModule = null;

        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.type == Module.typeModule.weapon)
            {
                if (maxLevel < upgrade.level)
                {
                    // On recupère le module le plus amélioré
                    maxLevel = upgrade.level;
                    if (upgrade.nextUpgrade != null)
                    { nextModule = upgrade.nextUpgrade.moduleName; }
                }
            }

            // Si le module existe déjà, ne pas l'ajouter.
            if (upgrade.moduleName ==  module.moduleName)
            {
                PlayMusic(wrong);

                return;
            }


        }


        if ((maxLevel + 1) != module.level)
        {
            PlayMusic(wrong);
            return;
        }

        if (maxLevel >= module.level)
        {
            return;
        }


        // Est-ce que l'amelio est du bon type
        if (upgrades.Count() > 0)
        {
            if (nextModule != null)
            {
                if (nextModule != module.moduleName)
                {
                    PlayMusic(wrong);

                    Debug.Log("Ce n'est pas le bon module");
                    return;
                }

            }

        }



        int goldPlayer = ResourceManager.instance.CheckGold();

        goldUpgrades -= module.price;

        if (goldPlayer < Mathf.Abs(goldUpgrades)) // Comparaison
        {

            // Non, pas assez de gold
            Debug.Log("Pas assez de gold sur le player");
            goldUpgrades += module.price;

            PlayMusic(wrong);
            animatorPanel.SetTrigger("NoGold"); //Doesn't work because Time.timeScale = 0
            

        }
        else
        {
            // Oui, On peut ajouter le module
            SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
            SlotSelected[] slotSelected = panelWeapon.GetComponentsInChildren<SlotSelected>();

            PlayMusic(clickMenu);

            // S'il y a déjà 3 modules, ne pas en ajouter un en plus

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

                    //slotSelected[i].SetModule(module); To sell slot

                    break; // Quand un slot est vide, terminé la boucle
                }
            }
            else
            {

            }


            // Mettre à jour l'amelioration sur la mode
            upgrade.UpdateTowerInMenu(module.prefab);

            foreach (Upgrade u in upgrades)
            {

                // Si le module existe déjà, ne pas l'ajouter.
                if (u.moduleName == module.moduleName)
                {
                    continue;
                }
                Debug.Log(u.moduleName);
                upgrade.UpdateComponent(u.module);

            }

            modulesToAdd.Add(module);

            

            if (modulesToAdd.Count > 0)
            {

                for (int i = 0; i < modulesToAdd.Count;i++)
                {
                    bool isSameName = false;
                    foreach (Upgrade u in upgrades)
                    {
                        if (u.moduleName == modulesToAdd[i].moduleName)
                        { isSameName = true; }
                    }
                    if (!isSameName)
                    {
                        //Debug.Log(modulesToAdd[i].moduleName);
                        upgrade.UpdateComponent(modulesToAdd[i]);
                    }
                    isSameName = false;
                }
            }



        }
        UpdateGoldUpgrade();

        // Update affichage menu
        // Mettre les images et les stat Module des modules dans les emplacements tour ou stat


        // Est-ce que c'est une amelio de niveau 1, 2? 

        // (est-ce qu'on peut mettre l'amelio ?




        // Regarder si on a assez de gold

        // Si cancel : erase modules du menu


    }

    //public void SaveInventory()
    //{
    //    // Si sauvegarde : Save dans Upgrades de la tour gameObject
    //    Debug.Log("on sauvegarde");

    //    // Creer les nouvelles tours avec les upgrade
    //    upgrade.CreateNewTower();




    //    // Deslectionné tous les slot
    //    SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
    //    for (int i = 0; i < slotImages.Length; i++)
    //    {
    //        slotImages[i].gameObject.GetComponent<Image>().enabled = false;
    //        Debug.Log("on remets les images du menu à zero");
    //    }
    //}

    public void SaveInventory()
    {
        // Si sauvegarde : Save dans Upgrades de la tour gameObject

        // Creer les nouvelles tours avec les upgrade
        PlayMusic(clickMenu);

        upgrade.CreateNewTower();

        // Use or add golds from resources
        if (goldUpgrades > 0)
        {
            ResourceManager.instance.AddGold(Mathf.Abs(goldUpgrades));
        }
        else
        {
            ResourceManager.instance.UseGold(Mathf.Abs(goldUpgrades));
        }

        // Reset menu Upgrade
        goldUpgrades = 0;
        UpdateGoldUpgrade();

        // Deslectionné tous les slot
        SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].gameObject.GetComponent<Image>().enabled = false;
        }

        modulesToAdd.Clear();

    }

    public void Unddo()
    {
        PlayMusic(clickMenu);

        modulesToAdd.Clear();

        // Unddo all modifications and get the info from scene 
        upgrade.GetTowerFromGame();
        GameObject towerInGame = upgrade.GetTowerInMenu();
        Upgrade[] upgrades = towerInGame.GetComponentsInChildren<Upgrade>();
        foreach (Upgrade u in upgrades)
        {
            modulesToAdd.Add(u.module);
        }

        SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].gameObject.GetComponent<Image>().enabled = false;
        }

        // reset gold used for upgrades
        goldUpgrades = 0;
        UpdateGoldUpgrade();

        ShowUpgradesInMenu(true);

    }

    private void OpenInventory()
    {

        ShowUpgradesInMenu(true);

    }

    public void ShowUpgradesInMenu(bool enable)
    {
        // Get all upgrades found in turret (which is on TurretSlot)
        upgrades = upgrade.GetComponentsInChildren<Upgrade>();

        // Put all upgrade in the right slot in menu
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.type == typeModule.weapon)
            {
                // Weapon modules 


                SlotImage[] slotImages = panelWeapon.GetComponentsInChildren<SlotImage>();
                for (int i = 0; i < slotImages.Length; i++)
                {
                    // Si le slot est déjà plein, passer au suivant
                    if (slotImages[i].gameObject.GetComponent<Image>().enabled)
                        continue;

                    // Changer l'image sur l'affichage du menu
                    slotImages[i].gameObject.GetComponent<Image>().enabled = enable;
                    slotImages[i].gameObject.GetComponent<Image>().sprite = upgrade.sprite;

                    //Debug.Log(slotImages[i].name);

                    break; // Quand un slot est vide, terminé la boucle
                }


            }
            else if (upgrade.type == typeModule.stat)
            {
                // Stat modules


            }

        }
    }


    public void UpdateGoldUpgrade()
    {
        if(goldUpgrades == 0)
        {
            goldUpgText.text = "";
        }
        else if (goldUpgrades > 0)
        {   
            goldUpgText.text = ("+ " + goldUpgrades.ToString());
        }
        else
        {
            // inf à 0
            goldUpgText.text = ("- " + Mathf.Abs(goldUpgrades).ToString());
        }
    }

    public void SellModule()
    {
        Debug.Log("click on module");
        GameObject Button = EventSystem.current.currentSelectedGameObject;
        Debug.Log(Button.name);

        // Enlève de l'affichage


        // Update le gold 


        // Enlève l'upgrade de la tour


        // Change de préfab


    }

    public void HoverSlot(bool enable)
    {
        if (panelStatModule != null)
        {

            if (enable == true)
            { 
                animatorPanel.SetTrigger("ModuleAppear"); 
                panelStatModule.SetActive(enable);

            }

            else
            {
                //animatorPanel.SetTrigger("ModuleDisappear");
                panelStatModule.SetActive(enable);
            }

        }
    }

    private IEnumerator DisappearingModule()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        panelStatModule.SetActive(false);
    }

    private void PlayMusic(string music)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(music);
        }
    }

}
