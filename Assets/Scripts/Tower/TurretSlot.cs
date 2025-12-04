using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class TurretSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private GameObject Turret;
    [Header("OffSet pour bien placer la tourelle")]
    //public Vector3 PlacePosition; DEL LEA -- 
    //public Vector3 Offset;        DEL LEA -- 
    public BuildManager buildManager;
    [SerializeField] private Transform spawnPointTurret;

    [Header("UI")]
    [SerializeField] private CanvasRenderer panelEmpty;
    [SerializeField] private CanvasRenderer panelOccupied;
    [SerializeField] private List<CanvasRenderer> panels;

    [Header("Feedback")]
    public Color HoverColor;
    public Color OriginalColor;
    private Renderer rend;          
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginalColor = Color.green;
        rend = GetComponentInChildren<Renderer>(); 
        buildManager = FindAnyObjectByType<BuildManager>(); // Not needed

        if (buildManager == null)
        {
            Debug.Log("Cannot find the BuildManager");
        }

        CanvasRenderer[] slotPanels = GetComponentsInChildren<CanvasRenderer>();
        foreach (CanvasRenderer panel in slotPanels)
        {
            if (panel.gameObject.CompareTag("SlotPanel"))
            {
                panel.gameObject.SetActive(false);
                panels.Add(panel);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (Turret != null)
        //    return;
        //if (!buildManager.BuildMode)
        //    return;

        rend.material.color = HoverColor;


        // Add code for tower information in HUD
        if (Turret == null)
        { return;  }
        else 
        {

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        //if(Turret != null)
        //{
        //    return;
        //}
        //if (!buildManager.BuildMode)
        //    return;
        //GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();

        // DEL LEA -- 
        //Turret = (GameObject)Instantiate(turretToBuild, transform.position + Offset, transform.rotation);

        //BEG LEA ++
        //Turret = (GameObject)Instantiate(turretToBuild, transform);
        //if (spawnPointTurret != null)
        //    Turret.transform.position = spawnPointTurret.position;
        // END LEA ++

        //buildManager.DisableCanvas();
        //buildManager.BuildMode = false;


        if (SlotPanelManager.instance != null)
        {
            // Close panels before opening another one
            ClosePanel();

            foreach (CanvasRenderer panel in panels)
            {
                // Add the panels of the slot to manage their visibility
                SlotPanelManager.instance.GetPanels(panel);
            }

            // Activate the right panel depending on the availibity on the slot
            if (Turret == null)
            {
                // Slot empty
                panelEmpty.gameObject.SetActive(true);
            }
            else
            {   //Slot occupied
                panelOccupied.gameObject.SetActive(true);
            }

        }
        else
        {
            Debug.Log("Put a Slot Panel Manager in the scene");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = OriginalColor;
    }

    public void PlaceTower(GameObject towerBase)
    {
        Debug.Log("Place tower");
        Turret = (GameObject)Instantiate(towerBase, transform);
        if (spawnPointTurret != null)
            Turret.transform.position = spawnPointTurret.position;

        ClosePanel();

        // Pay the price of the tower
        ResourceManager.instance.UseNuclear();

    }

    public void UpgradeTower()
    {
        Debug.Log("Upgrade Tower");

        ClosePanel();
    }

    public void SellTower()
    {
        Debug.Log("Sell Tower");

        ClosePanel();
    }

    public void MoveTower()
    {
        Debug.Log("Move Tower");
        ClosePanel();
    }


    private void ClosePanel()
    {
        if (SlotPanelManager.instance != null)
        {
            SlotPanelManager.instance.ClosePanel();
        }
    }
}
