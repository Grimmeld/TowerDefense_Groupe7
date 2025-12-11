using UnityEngine;

public class NuclearImage : MonoBehaviour
{
    private void Start()
    {
        if (ResourceManager.instance != null)
            ResourceManager.instance.RegisterImage(gameObject);
    }

    /*private void OnDisable()
    {
        if (ResourceManager.instance != null)
            ResourceManager.instance.UnregisterImage(gameObject);
    }

    private void OnDestroy()
    {
        if (ResourceManager.instance != null)
            ResourceManager.instance.UnregisterImage(gameObject);
    }*/
}