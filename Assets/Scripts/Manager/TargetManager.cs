using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;
    private List<GameObject> AvailableEnemies = new List<GameObject>();
    private List<GameObject> TargetedEnemies = new List<GameObject>();
    public void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void RegisterEnemy(GameObject enemy) // fonction pour register un ennemi dans la liste des ennemi disponible
    {
        if(!AvailableEnemies.Contains(enemy))
        {
            AvailableEnemies.Add(enemy);
        }
    }

    public void UnRegisterEnemy(GameObject enemy)//pour enlever de la liste les ennemi qui meurt
    {
        if (!AvailableEnemies.Remove(enemy))
        {
            TargetedEnemies.Remove(enemy);
        }
    }

    public Transform GetFreeTarget()  //pour que le missible puisse fetch une cible qui est libre
    {
        foreach(var enemy in AvailableEnemies)
        {
            if(!TargetedEnemies.Contains(enemy))
            {
                TargetedEnemies.Add(enemy);
                return enemy.transform;
            }
           
        }
        return null;
    }

    public void ReleaseTarget(GameObject enemy) //Si le missile touche l'ennemi, l'enlever de la liste des ennemi ciblé
    {
        TargetedEnemies.Remove(enemy);
    }
}
