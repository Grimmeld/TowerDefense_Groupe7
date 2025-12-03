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

    //BEG LEA ++

    // Activation of the tower depends on the supply power of the zone
    private void OnEnable()
    {
        EnableScript(true);
    }

    private void OnDisable()
    {
        EnableScript(false);
    }
    //END LEA ++ 
}
