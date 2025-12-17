using System.Collections;
using UnityEngine;

public class StartMenuSplashscreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public GameObject panel;

    public string whip;

    void Start()
    {
        if (animator != null)
        {
            animator.SetTrigger("Start");
        }

        StartCoroutine("Whipping");
        StartCoroutine("DisablingPanel");
    }
    
    private IEnumerator DisablingPanel()
    {

        yield return new WaitForSeconds(2.5f);
        panel.SetActive(false);
    }

    private IEnumerator Whipping()
    {

        yield return new WaitForSeconds(1.3f);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(whip);
        }
    }
}
