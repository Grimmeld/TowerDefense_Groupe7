using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SlotPanelManager : MonoBehaviour
{
    public static SlotPanelManager instance;

    [SerializeField] private List<CanvasRenderer> slotPanels;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;


    }

    public void GetPanels(CanvasRenderer panel)
    {
        slotPanels.Add(panel);
    }

    public void ClosePanel()
    {
        if (slotPanels.Count <= 0)
        {
            return;
        }

        // Close panel
        foreach (CanvasRenderer slot in slotPanels)
        {
            if (slot.gameObject.activeSelf)
            {   
                slot.gameObject.SetActive(false);
            }
        }
        
        slotPanels.Clear();
    }

    public void DisablePanels(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ClosePanel();
        }
    }

    public int GetPanelsCount()
    {
        return slotPanels.Count;
    }
}
