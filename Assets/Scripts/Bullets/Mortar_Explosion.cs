using UnityEngine;

public class Mortar_Explosion : MonoBehaviour
{
    public float duration;
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
