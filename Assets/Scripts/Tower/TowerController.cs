using UnityEngine;

public class TowerController : MonoBehaviour
{
    public MonoBehaviour attackScript;
    public Tower_Animation tower_Animation;

    private void Awake()
    {
        attackScript = GetComponent<MonoBehaviour>();
        tower_Animation = GetComponent<Tower_Animation>();
    }
    public void EnableScript(bool enable)
    {
        attackScript.enabled = enable;
        if (tower_Animation != null)
        {
            tower_Animation.enabled = enable;
        }
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
        tower_Animation.animator.SetTrigger("Disable");
    }
    //END LEA ++ 
}
