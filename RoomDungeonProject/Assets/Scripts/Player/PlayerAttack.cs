using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private PlayerMove playerMove;

    private bool isAttacking = false;

    [Header("�ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Attack1Ani";

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }


    public void InputAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        if (isAttacking) 
        {
            return;
        }
        // ����ó��
        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
            playerMove.moveSpeed = 0;
        }

        StartCoroutine(AttackCoolDownByAni());

    }

    // ���� ������ �ֱ�
    private IEnumerator AttackCoolDownByAni()
    { 
        isAttacking = true;        
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false;

    }

}
