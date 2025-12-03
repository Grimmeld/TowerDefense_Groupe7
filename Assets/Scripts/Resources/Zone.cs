using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class Zone : MonoBehaviour
{
   public List<Zone> adjacentZones = new List<Zone>();
    public bool isPowered;
    public bool isEnabled;
    public bool isSource;

    public Color activatedColor;
    public Color disabledColor;



    private void Start()
    {
        UpdateVisual();
    }

    public void SetPowered(bool state)
    {
        isPowered = state;
        UpdateVisual();
    }


    public void ToggleEnabled()
    {
        isEnabled = !isEnabled;

        if (isEnabled == false)
        {
            isPowered = false;
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (!isEnabled)
        {
            GetComponent<Renderer>().material.color = Color.red; // désactivée
        }
        else if (isEnabled && !isPowered)
        {
            GetComponent<Renderer>().material.color = Color.yellow; // activée mais non alimentée
        }
        else // isEnabled && isPowered
        {
            GetComponent<Renderer>().material.color = Color.green; // activée et alimentée
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //Get all zones adjacent to current zone
        if (other.gameObject.TryGetComponent(out Zone zone))
        {
            // Add to list
            adjacentZones.Add(zone);
        }
    }
}
