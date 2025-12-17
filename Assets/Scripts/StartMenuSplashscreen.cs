using System.Collections;
using UnityEngine;

public class StartMenuSplashscreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public GameObject panel;

    void Start()
    {
        if (animator != null)
        {
            animator.SetTrigger("Start");
        }

        StartCoroutine("DisablingPanel");
    }
    
    private IEnumerator DisablingPanel()
    {

        yield return new WaitForSeconds(2.5f);
        panel.SetActive(false);
    }

}
