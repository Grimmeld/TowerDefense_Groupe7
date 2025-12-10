using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Module;

public class Upgrade : MonoBehaviour
{
    public string moduleName;
    public Sprite sprite;
    public GameObject prefab;
    public TowerStats stats;
    public typeModule type;
    public typeTower tower;
    public string description;
    public int level;
    public int price;
    public int sell;
    public Module nextUpgrade;
    public Module module;


    public void SetStat(Module stat)
    {
        moduleName = stat.moduleName;
        sprite = stat.sprite;
        description = stat.description;
        type = stat.type;
        level = stat.level;
        tower = stat.tower;
        nextUpgrade = stat.nextUpgrade; 
        module = stat;
    }
    

    

}
