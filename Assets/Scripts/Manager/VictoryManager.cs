using UnityEngine;
using UnityEngine.InputSystem;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Defeat")]
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Animator defeatAnimator;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Sound")]
    [SerializeField] private string clic;

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

    public void Victory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        Time.timeScale = 0.0f;
        
    }

    public void OnClic()
    {
        if (AudioManager.instance != null && clic != null)
        {
            AudioManager.instance.Play(clic);
        }
    }

    public void DebugTool(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
        }
    }
}
