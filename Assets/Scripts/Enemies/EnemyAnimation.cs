using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private bool IsDead;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            //Debug.Log("No animator on : " + this.gameObject.name);
        }
    }

    private void Update()
    {
        // 
        if (IsDead)
        {
            if (animator != null)
            {   // Animation change the next frame
                // We need to wait for the frame after the death occured to get the time
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death_Vehicule")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack"))
                    Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void CheckDeath(bool state)
    {
        IsDead = state;
    }

    public void PlayAnimation(string  name)
    {
        if (animator != null)
        {
            animator.Play(name);
        }
    }

}
