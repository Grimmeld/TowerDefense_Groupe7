using UnityEngine;

public class Sapper_Animation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool IsDead;
    public Robot_State State;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        State = Robot_State.Robot;
        animator = GetComponentInChildren<Animator>();
        animator.Play("Vehicle_Move");
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vehicle_Move"))
        {
            State = Robot_State.vehicle;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vehicle_To_Robot_Attack"))
        {
            State = Robot_State.Attack;
        }
        // 
        if (IsDead)
        {
            if (animator != null)
            {   // Animation change the next frame
                // We need to wait for the frame after the death occured to get the time
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vehicle_Death")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Death_Attack")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Robot_Death"))
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

    public void PlayAnimation(string name)
    {
        if (animator != null)
        {
            animator.Play(name);
        }
    }

    public void PlayDeath()
    {
        if (State == Robot_State.Robot)
        {
            PlayAnimation("Robot_Death");
        }
        else if (State == Robot_State.vehicle)
        {
            PlayAnimation("Vehicle_Death");
        }
        else if (State == Robot_State.Attack)
        {
            PlayAnimation("Death_Attack");
        }
    }
}
