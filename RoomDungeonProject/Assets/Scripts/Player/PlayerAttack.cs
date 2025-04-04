using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private PlayerMove playerMove;

    public bool isAttacking = false;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Attack1Ani";

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }


    public void InputAttack()
    {
        if (Input.GetButtonDown("Fire1") && playerMove.isRolling == false)
        {
            PerformAttack();

            Invoke("AttackShake", 0.3f);
            
        }
    }

    void AttackShake()
    {
        StartCoroutine(CameraManager.instance.Shake());
    }

    public void PerformAttack()
    {
        if (isAttacking) 
        {
            return;
        }
        // 예외처리
        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
        }

        StartCoroutine(AttackCoolDownByAni());

    }

    // 공격 딜레이 넣기
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

    public void AttackSound()
    {
        SoundManager.instance.PlaySFX(SFXType.Attack1Sound);
    }

}
