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
    [SerializeField] private ActivateTower activateTower;


    // Feedback on interaction
    [Header("Feedback")]
    [SerializeField] GameObject antennaMesh;
    [SerializeField] private Color hoverColor, normalColor;
    [SerializeField] private Material activatedColor, disableColor;

    private void Awake()
    {
        GetComponentInChildren<Renderer>().material.color = normalColor;
        activateTower = GetComponentInParent<ActivateTower>();

        if (activateTower.CheckActivation())
        {
            antennaMesh.GetComponent<Renderer>().material = activatedColor;
        }
        else
        {
            antennaMesh.GetComponent<Renderer>().material = disableColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<Renderer>().material.color = hoverColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (activateTower != null)
        {
            if (activateTower.CheckActivation())
            {
                // Transmitter is activated -> Disable it
                activateTower.ChangeActivation();

                //Feedback
                antennaMesh.GetComponent<Renderer>().material = disableColor;

            }
            else
            {
                // Transmitter is not actived -> Try to activate it
                if (ResourceManager.instance.CheckNuclear())
                {
                    activateTower.ChangeActivation();

                    //Feedback
                    antennaMesh.GetComponent<Renderer>().material = activatedColor;


                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) 
    {

        GetComponentInChildren<Renderer>().material.color = normalColor;
    }

    public void SetActivateTower(ActivateTower activation)
    {
        activateTower = activation; 
    }

}
