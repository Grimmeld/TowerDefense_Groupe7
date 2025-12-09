using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "Module", menuName = "Scriptable Objects/Module")]
public class Module : ScriptableObject
{
    public string moduleName;
    public Sprite image;
    public GameObject prefab;
    public TowerStats stats;
    public typeModule type;
    public typeTower tower;
    public string description;

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

