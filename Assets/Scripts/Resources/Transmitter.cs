using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transmitter : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public Zone zone;
    public PowerManager manager;

    [Header("Feedback")]
    [SerializeField] private Renderer antennaRend;  //Change color when enabled
    [SerializeField] private Material enabledMat, disabledMat;

    private void Start()
    { 
        if (manager == null)
        {
            manager = FindAnyObjectByType<PowerManager>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (zone.isSource)
        { return; } // Don't click if Nuclear core

            zone.ManagingActivation();
            manager.RecalculatePower();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void EnableTransmitter()
    {
        if (antennaRend != null)
        {
            antennaRend.material = enabledMat;
        }

    }

    public void DisableTransmitter()
    {
        if (antennaRend != null)
        {
            antennaRend.material = disabledMat;
        }

    }
}
