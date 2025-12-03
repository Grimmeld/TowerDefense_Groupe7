using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PowerTransmitter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    // Script on transmitter

    [Header("Activation")]
    [SerializeField] private ActivateZone activateZone;


    // Feedback on interaction
    [Header("Feedback")]
    [SerializeField] GameObject antennaMesh;
    [SerializeField] private Color hoverColor, normalColor;
    [SerializeField] private Material activatedColor, disableColor;


    private void Awake()
    {
        GetComponentInChildren<Renderer>().material.color = normalColor;
        activateZone = GetComponentInParent<ActivateZone>();

        if (activateZone.CheckActivation())
        {
            antennaMesh.GetComponent<Renderer>().material = activatedColor;
        }
        else
        {
            antennaMesh.GetComponent<Renderer>().material = disableColor;
        }
    }

    // HOVER
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<Renderer>().material.color = hoverColor;
    }


    //CLICK
    public void OnPointerDown(PointerEventData eventData)
    {
        if (activateZone != null)
        {
            if (activateZone.CheckActivation())
            {
                // Transmitter is activated -> Disable it
                activateZone.ChangeActivation();

                activateZone.DisableAdjacentZone();

                //Feedback
                //antennaMesh.GetComponent<Renderer>().material = disableColor;

            }
            else
            {
                // Transmitter is not actived -> Try to activate it
                if (ResourceManager.instance.CheckNuclear())
                {
                    // We got enough resource
                    if (activateZone.CheckAdjacentZones())
                    {

                        activateZone.ChangeActivation();

                        //Feedback
                        antennaMesh.GetComponent<Renderer>().material = activatedColor;
                    }


                }
            }
        }
    }


    //UNHOVER
    public void OnPointerExit(PointerEventData eventData) 
    {

        GetComponentInChildren<Renderer>().material.color = normalColor;
    }

    public void SetActivateTower(ActivateZone activation)
    {
        activateZone = activation; 
    }

    public void ChangeFeedbackAntenna()
    {
        antennaMesh.GetComponent<Renderer>().material = disableColor;
    }

}
