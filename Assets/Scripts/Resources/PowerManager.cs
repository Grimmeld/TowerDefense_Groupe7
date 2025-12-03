using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


public class PowerManager : MonoBehaviour
{
    public Zone sourceZone;
    public List<Zone> allZones = new List<Zone>();

    private void Awake()
    {
        // Get all the zones in the map
        if (allZones == null || allZones.Count == 0)
        {
            //allZones = new List<Zone>(FindObjectsOfType<Zone>());
            Zone[] zones = GetComponentsInChildren<Zone>();
            foreach(Zone z in zones)
            {
                allZones.Add(z);
            }
        }

        // Get Nuclear Core
        if (sourceZone == null)
        {
            foreach (var z in allZones)
            {
                if (z.isSource)
                {
                    sourceZone = z;
                    break;
                }
            }
        }
    }

    private void Start()
    {
        RecalculatePower();
    }

    public void RecalculatePower()
    {
        foreach (Zone z in allZones)
        {
            z.SetPowered(false);
        }

        if(sourceZone == null)
        {
            Debug.Log("Pas de source central");
        }

        if (!sourceZone.isEnabled)
        {
            Debug.Log("Source centrale n'est pas activée");
        }

            Queue<Zone> queue = new Queue<Zone>();
            

            sourceZone.SetPowered(true);
            queue.Enqueue(sourceZone);

        while (queue.Count > 0)
            {
                Zone current = queue.Dequeue();

                foreach (Zone adj in current.adjacentZones)
                {

                if (!adj.isPowered && adj.isEnabled)
                    {

                        adj.SetPowered(true);
                        queue.Enqueue(adj);
                    }

                }
            }

        
    }


}

