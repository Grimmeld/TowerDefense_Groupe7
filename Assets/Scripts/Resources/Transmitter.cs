using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transmitter : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public Zone zone;
    public PowerManager manager;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        zone.ToggleEnabled();
        manager.RecalculatePower();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
