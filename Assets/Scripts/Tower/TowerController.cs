using UnityEngine;

public class TowerController : MonoBehaviour
{
    public MonoBehaviour attackScript;

    private void Awake()
    {
        attackScript = GetComponent<MonoBehaviour>();
    }
    public void EnableScript(bool enable)
    {
        attackScript.enabled = enable;
    }
}
