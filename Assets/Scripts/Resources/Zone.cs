using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class Zone : MonoBehaviour
{
    [Header("Supply power zone")]
    public List<Zone> AdjacentZones = new List<Zone>();
    public bool isPowered;
    public bool isEnabled;
    public bool isSource;

    [Header("Elements in Zone")]
    [SerializeField] private TurretSlot[] turretSlots;
    private Transmitter transmitter;

    [Header("Feedback")]
    [SerializeField] private List<GameObject> activationEffects;   // Activated when zone is powered




    private void Start()
    {
        turretSlots = GetComponentsInChildren<TurretSlot>();
        transmitter = GetComponentInChildren<Transmitter>();

        // For now the Effects are lights in the zone
        Light[] lights = GetComponentsInChildren<Light>();
        foreach(Light light in lights)
        {
            activationEffects.Add(light.gameObject);
            light.gameObject.SetActive(isEnabled);  
        }

        UpdateVisual();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Get all zones adjacent to current zone
        if (other.gameObject.TryGetComponent(out Zone zone))
        {
            // Add to list
            AdjacentZones.Add(zone);
        }
    }

    public void SetPowered(bool state)
    {
        // Set power
        isPowered = state;

        //Set state slot and towers
        foreach (TurretSlot slot in turretSlots)
        {
            TowerController towerController = slot.GetComponentInChildren<TowerController>();
            if (towerController != null)
            {
                towerController.enabled = state;
            }
            // BEG LEA -- 
            //BaseTower baseTower = slot.GetComponentInChildren<BaseTower>();
            //if (baseTower != null)
            //{
            //    baseTower.enabled = state;
            //}
            // END LEA --
        }

        UpdateVisual();
    }



    public void ManagingActivation()
    {
        if (isEnabled)
        {
            // Zone is activated => We want to disable it
            // => Get one nuclear resource
            ResourceManager.instance.StoreNuclear();

            // Change state 
            // Change visual
            ToggleEnabling();
        }
        else
        {
            // Zone is not activated => We want to activate it
            // Check resource
            if (ResourceManager.instance != null
                && ResourceManager.instance.CheckNuclear())
            {
                // Take resource if enough
                ResourceManager.instance.UseNuclear();

                // Change state 
                // Change visual
                ToggleEnabling();

            }
            else
            {
                //else do nothing
            }

        }


    }

    private void ToggleEnabling()
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
            if (transmitter != null)
            {
                transmitter.DisableTransmitter();
            }
        }
        else if (isEnabled && !isPowered)
        {
            GetComponent<Renderer>().material.color = Color.yellow; // activée mais non alimentée
            if(transmitter != null)
            {
                transmitter.EnableTransmitter();
            }
            
        }
        else // isEnabled && isPowered
        {
            GetComponent<Renderer>().material.color = Color.green; // activée et alimentée
            if (transmitter != null)
            {
                transmitter.EnableTransmitter();
            }
        }

        ActivationLights(isPowered);
    }

    private void ActivationLights(bool state)
    {
        foreach(GameObject effect in activationEffects)
        {
            effect.SetActive(state);
        }
    }

}
