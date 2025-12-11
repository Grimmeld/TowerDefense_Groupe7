using UnityEngine;

[CreateAssetMenu(fileName = "Module", menuName = "Scriptable Objects/Module")]
public class Module : ScriptableObject
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

    public enum typeModule
    {
        stat,
        weapon
    }

    public enum typeTower
    {
        air,
        ground
    }


}

