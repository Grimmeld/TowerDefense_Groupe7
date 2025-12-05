using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretUpgrade : MonoBehaviour
{
    public void Upgrade(GameObject tower)
    {
        Debug.Log("Upgrade : " + tower.name);
    }
}
