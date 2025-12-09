using System;
using System.Collections;
using UnityEngine;

public enum Turret_State { Attacking, Idling }
public class Tower_Animation : MonoBehaviour
{
    public Turret_State State;
    [SerializeField] public Animator animator;
    bool Idling = false;
    bool prepareAttack = false;
    bool finishedAttack = false;
    bool animationFinished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public event Action OnAttackFire;
    void Start()
    {
        animator.Play("Tower_Arrive");
        Invoke(nameof(animFinish), 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(animationFinished)
        {
            if (State == Turret_State.Attacking)
            {
                if (!prepareAttack)
                {
                    prepareAttack = true;
                    Idling = false;
                    IdleAttack();
                }
                if (finishedAttack)
                {
                    finishedAttack = false;
                    StartCoroutine(PlayAttackAndNotify());
                }
            }
            if (State == Turret_State.Idling)
            {
                if (!Idling)
                {
                    Idling = true;
                    finishedAttack = false;
                    prepareAttack = false;
                    ReturnToIdle();
                }
            }
        }

    }

        

    public void IdleAttack()
    {
        animator.Play("Tower_Idle_To_Attack");
        finishedAttack = true;
    }
    public void Attack()
    {

        animator.Play("Tower_Attack");
    }
    public void ReturnToIdle()
    {
        animator.Play("Tower_Attack_To_Idle");
    }

    private IEnumerator PlayAttackAndNotify()
    {
        animator.Play("Tower_Attack");
        yield return null;
        float length = 0f;
        var clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips != null && clips.Length > 0 && clips[0].clip != null)
        {
            length = clips[0].clip.length;
        }
        if(length > 0f)
        {
            yield return new WaitForSeconds(length);
        }
        else
        {
            yield return null;
        }
        OnAttackFire.Invoke();
        prepareAttack = false;
        finishedAttack = false;
    }

    void animFinish()
    {
               animationFinished = true;
    }
}
