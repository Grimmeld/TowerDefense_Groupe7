using UnityEngine;

public enum Robot_State { Robot, vehicle, Attack }
public class EnemyAnimation : MonoBehaviour
{
    public Robot_State State;
    [SerializeField] private Animator animator;
    [SerializeField] private bool IsDead;

    private void Start()
    {
        State = Robot_State.Robot;
        animator = GetComponentInChildren<Animator>();
        PlayAnimation("Robot_Walk");
        if (animator == null)
        {
            //Debug.Log("No animator on : " + this.gameObject.name);
        }
    }

    private void Update()
    {
        if (animator == null) return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vehicle_Move"))
        {
            State = Robot_State.vehicle;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vehicle_To_Robot_Attack"))
        {
            State = Robot_State.Attack;
        }
        else
        {
            State = Robot_State.Robot;
        }

        if (IsDead)
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Vehicle_Death")
                || info.IsName("Death_Attack")
                || info.IsName("Robot_Death"))
            {
                Destroy(gameObject, info.length);
            }
        }
    }

    public void CheckDeath(bool state)
    {
        IsDead = state;
    }
    public void RefreshState()
    {
        if (animator == null) return;
        var info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Vehicle_Move"))
            State = Robot_State.vehicle;
        else if (info.IsName("Vehicle_To_Robot_Attack"))
            State = Robot_State.Attack;
        else
            State = Robot_State.Robot;
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
        if (animator == null)
        {
            CheckDeath(true);
            return;
        }

        if (State == Robot_State.Robot)
        {
            animator.Play("Robot_Death");
        }
        else if (State == Robot_State.vehicle)
        {
            animator.Play("Vehicle_Death");
        }
        else if (State == Robot_State.Attack)
        {
            animator.Play("Death_Attack");
        }

        CheckDeath(true);
    }

}