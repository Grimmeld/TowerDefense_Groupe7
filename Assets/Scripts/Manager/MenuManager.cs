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
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LD_Scene_LA_V2");
    }
    public void Zoo()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Zoo 1");
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu_Main");
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
