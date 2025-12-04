using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundClosePanel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        
        Debug.Log("Clic panel");

    }
}
