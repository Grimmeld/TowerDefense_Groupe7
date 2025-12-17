using UnityEngine;
using UnityEngine.InputSystem;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [SerializeField] private GameObject defeatPanel;

    [SerializeField] private Animator defeatAnimator;

    private void Start()
    {
        if (instance != null)
            return;

        instance = this;
    }

    public void Defeat()
    {
        // Stop the game
        Time.timeScale = 0.0f;

        // Play animator to show canvas
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            defeatAnimator.SetTrigger("DefeatTrig");
        }
    }

    public void DebugTool(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Defeat();
        }
    }
}
