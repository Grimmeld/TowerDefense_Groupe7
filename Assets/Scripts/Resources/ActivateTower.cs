using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTower : MonoBehaviour
{
    //Script on zones
    // Zones will manage the activation of the tower
    //[SerializeField] private List<TurretSlot> _turretSlots; // Slots in zone
    [SerializeField] private TurretSlot[] _turretSlots; // Slots in zone
    [SerializeField] private PowerTransmitter powerTransmitter;           // Transmitter in zone    
    [SerializeField] private bool isActivated;

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
        if (isActivated == false)
        {
            ResourceManager.instance.UseNuclear();
        }
        else
        {
            ResourceManager.instance.StoreNuclear();
        }
        
        isActivated = !isActivated;
        EnableTowers(isActivated);
    }



    private void OnTriggerEnter(Collider other)
    {
        // Get all slots that are in the zone
        //if (other.gameObject.TryGetComponent(out TurretSlot turretSlot))
        //{
        //    // Add to list
        //    _turretSlots.Add(turretSlot);
        //}

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
