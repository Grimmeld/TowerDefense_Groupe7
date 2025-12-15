using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("LD_Scene_LD_V2");
    }
    public void Zoo()
    {
        SceneManager.LoadScene("Zoo");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits()
    {
        
    }
    public void Options()
    {

    }
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
        {
        Time.timeScale = 1f;
        }
}
