using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string click;

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
        PlayMusic(click);
        Time.timeScale = 0f;
    }
    public void Resume()
        {
        PlayMusic(click);
        Time.timeScale = 1f;
        }

    public void Settings()
    {
        PlayMusic(click);
    }

    public void ToMainMenu()
    {
        PlayMusic(click);
        SceneManager.LoadScene("Menu_Main");
    }

    private void PlayMusic(string name)
    {
        if (AudioManager.instance != null && name != null)
        {
            AudioManager.instance.Play(name);
        }
    }
}
