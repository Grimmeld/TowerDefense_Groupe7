using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private GameObject Turret;
    [Header("OffSet pour bien placer la tourelle")]
    public Vector3 PlacePosition;
    public Vector3 Offset;
    BuildManager buildManager;
    public Canvas TurretChoiceCanvas;

    public Color HoverColor;
    public Color OriginalColor;
    private Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginalColor = Color.green;
        rend = GetComponent<Renderer>();
        buildManager = FindAnyObjectByType<BuildManager>();
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
        Turret = (GameObject)Instantiate(turretToBuild, transform.position + Offset, transform.rotation);
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
