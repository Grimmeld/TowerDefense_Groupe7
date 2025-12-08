using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    [SerializeField] private List<Module> modules; // Upgrade of the tower


    public List<Module> GetModules()
    {
        return modules;
    }



}
