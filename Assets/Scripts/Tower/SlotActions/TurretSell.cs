using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSell : MonoBehaviour
{
    public GameObject Sell(GameObject tower)
    {
        Destroy(tower.gameObject);

        if (ResourceManager.instance != null)
            ResourceManager.instance.StoreNuclear();

        return null;
    }
}
