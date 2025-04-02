using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {        
        animator.SetTrigger("Attack");
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void TriggerJumping()
    {
        animator.SetTrigger("Jump");
    }

    public void SetFalling(bool isFalling)
    {
        animator.SetBool("IsFalling", isFalling);
    }

    public void SetRun(bool isRun)
    {
        animator.SetBool("IsRun", isRun);
    }

    public void TriggerRolling()
    {
        animator.SetTrigger("Roll");
    }

}
