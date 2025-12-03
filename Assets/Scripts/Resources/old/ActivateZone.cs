using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class ActivateZone : MonoBehaviour
{
    //Script on zones
    // Zones will manage the activation of the tower
    //[SerializeField] private List<TurretSlot> _turretSlots; // Slots in zone
    [SerializeField] private TurretSlot[] _turretSlots; // Slots in zone
    [SerializeField] private PowerTransmitter powerTransmitter;           // Transmitter in zone    
    [SerializeField] private bool isActivated;

    [SerializeField] private List<ActivateZone> activateZones;

    // Feedback on interaction
    [Header("Feedback")]
    [SerializeField] private GameObject visualEffect;

    private void Awake()
    {
        // If the nuclear core is in the zone -> Always activated
        if (gameObject.CompareTag("Base"))
        {
            isActivated = true;
        }


        //Get the transmitter of the zone
        powerTransmitter = GetComponentInChildren<PowerTransmitter>();

        // Faire la même chose pour les slot GetComponentsInChildren - au lieu d'utiliser un trigger enter
        _turretSlots = GetComponentsInChildren<TurretSlot>();
    }


    public bool CheckActivation()
    {
        return isActivated;
    }

    public void ChangeActivation()
    {
        isActivated = !isActivated;


        if (isActivated)
        {
            ResourceManager.instance.UseNuclear();
        }
        else
        {
            ResourceManager.instance.StoreNuclear();
            powerTransmitter.ChangeFeedbackAntenna();
        }

        EnableTowers(isActivated);

        // Feedback
        visualEffect.SetActive(!isActivated); // Add a visual if the zone is not activated
    }

    public bool CheckAdjacentZones()
    {
        foreach (ActivateZone zone in activateZones)
        {
           if (zone.gameObject.CompareTag("Base"))
            {
                // Zone is adjacent to a Base Zone
                //Debug.Log("Zone is adjacent to the nuclear core ");

                return true;
            }
           else
            {
                //Debug.Log("Zone not adjacent to the nuclear core");
                if(zone.CheckActivation())
                {
                    if (zone.CheckAdjacentZonesV2(zone))
                    // One of the adjacent zone is activated -> Our zone can be activated
                    return true;
                }
                else {  }

                    
            }
        }
        return false;
    }


    public void DisableAdjacentZone()
    {
        //Send a message to the other zones
        foreach(ActivateZone zone in activateZones)
        {
            if (zone.gameObject.CompareTag("Base"))
            {   
                // No action on base zone
                Debug.Log("Nuclear core reached");
                continue;
            }

            if (!zone.CheckActivation())
            {
                Debug.Log(zone.name + " is not activated, no change to do");
                continue;
            }

            if (zone.CheckAdjacentZones())
            {
                Debug.Log(zone.name + " Can still be activated");
                // One of the adjacent zone is activated
                // The zone can still be activated
            }
            else
            {
                Debug.Log(zone.name + " Change activation");
                // No other zone is activated
                // The zone needs to be disabled
                zone.ChangeActivation();

            }

        }

    }

    public bool CheckAdjacentZonesV2(ActivateZone otherZone)
    {
        foreach (ActivateZone zone in activateZones)
        {
            if (zone.gameObject.CompareTag("Base"))
            {
                // Zone is adjacent to a Base Zone
                //Debug.Log("Zone is adjacent to the nuclear core ");

                return true;
            }
            else
            {
                Debug.Log("Adjacent V2 : " + zone.name + "VS" + otherZone.name);

                if (zone.name ==  otherZone.name)
                {
                    Debug.Log(otherZone.name + " Zone de départ, ne pas checker");
                    continue;
                }

                //Debug.Log("Zone not adjacent to the nuclear core");
                if (zone.CheckActivation())
                {
                    // One of the adjacent zone is activated -> Our zone can be activated
                    return true;
                }
                else { }


            }
        }
        return false;
    }



    private void OnTriggerEnter(Collider other)
    {
        //Get all zones adjacent to current zone
        if (other.gameObject.TryGetComponent(out ActivateZone activate))
        {
            // Add to list
            activateZones.Add(activate);
        }

    }



    private void EnableTowers(bool activation)
    {
        foreach (TurretSlot slot in _turretSlots)
        {
            BaseTower baseTower = slot.GetComponentInChildren<BaseTower>();
            if (baseTower != null)
            {
                baseTower.enabled = activation;
            }
        }
    }
}
