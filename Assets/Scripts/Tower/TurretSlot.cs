using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class TurretSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private GameObject Turret;
    [Header("OffSet pour bien placer la tourelle")]
    //public Vector3 PlacePosition; DEL LEA -- 
    //public Vector3 Offset;        DEL LEA -- 
    public BuildManager buildManager;
    [SerializeField] private Transform spawnPointTurret;



    [Header("Feedback")]
    public Color HoverColor;
    public Color OriginalColor;
    //private Renderer rend;          
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginalColor = Color.green;
        // rend = GetComponent<Renderer>(); DEL LEA -- 
        buildManager = FindAnyObjectByType<BuildManager>();

        if (buildManager == null)
        {
            Debug.Log("Cannot find the BuildManager");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Turret != null)
            return;
        if (!buildManager.BuildMode)
            return;
        GetComponent<Renderer>().material.color = HoverColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(Turret != null)
        {
            return;
        }
        if (!buildManager.BuildMode)
            return;
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();

        // DEL LEA -- 
        //Turret = (GameObject)Instantiate(turretToBuild, transform.position + Offset, transform.rotation);

        //BEG LEA ++
        Turret = (GameObject)Instantiate(turretToBuild, transform);
        if (spawnPointTurret != null)
            Turret.transform.position = spawnPointTurret.position;
        // END LEA ++

        buildManager.DisableCanvas();
        buildManager.BuildMode = false;

        // Pay the price of the tower
        ResourceManager.instance.UseNuclear();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Renderer>().material.color = OriginalColor;
    }
}
