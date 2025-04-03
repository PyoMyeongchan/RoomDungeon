using System.Collections;
using System.Linq.Expressions;
using Unity.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float jumpForce = 6.0f;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private float groundCheckDistance = 0.6f;
    private PlayerAnimation playerAnimation;
    
    public float rollSpeed = 1.0f;
    public float rollDuration = 0.5f;
    public bool isRolling = false;
    private bool airRolling = false;
  
    private PlayerAttack playerAttack;

    private Camera mainCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        mainCamera = Camera.main;
        playerAttack = GetComponent<PlayerAttack>();    
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (moveInput * moveSpeed, rb.linearVelocity.y);

        // 해당 위치의 레이어를 확인한다
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);
        
        // 예외 처리
        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
            
        }

        FlipPlayer();
             
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            playerAnimation.SetRun(moveInput != 0);
            moveSpeed = 5.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded)
        {
            playerAnimation.SetRun(false);
            moveSpeed = 3.0f;

        }


        // 점프상태
        //rigidbody2D에서 position X,Y 체크하면 점프가 안된다.
        if (Input.GetButtonDown("Jump") && isGrounded && !isRolling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.TriggerJumping();
            SoundManager.instance.PlaySFX(SFXType.JumpSound);

        }

        // 낙하상태
        if (!isGrounded)
        {
            playerAnimation.SetFalling(true);

        }
        else if (isGrounded)
        {            
            playerAnimation.SetFalling(false);

            airRolling = true;
        }

        

        // 구르기
        if (Input.GetMouseButtonDown(1) && !isRolling && playerAttack.isAttacking == false)
        {
            if (isGrounded || airRolling)
            {
                Rolling();

                // 점프시 구르기 한번만 가능하게 하기
                if (!isGrounded)
                {
                    airRolling = false;
                }
            }
        }

        // 구르기시 다른 동작 못하도록 설정
        if (isRolling)
        {
            return;
        }
        

    }

    // 마우스포인터로 좌우 구현
    void FlipPlayer()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void Rolling()
    {
        float rollDirection = GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        bool isGround = Physics2D.Raycast(transform.position, Vector2.right * rollDirection, groundCheckDistance, groundLayer);

        if (isGround) return;

        isRolling = true;
        playerAnimation.TriggerRolling();
        SoundManager.instance.PlaySFX(SFXType.RollSound);
        StartCoroutine(Roll(rollDirection));
    }

    IEnumerator Roll(float direction)
    {
        float elapsedTime = 0f;
        float currentGravity = rb.gravityScale;
        rb.gravityScale = 0;

        while (elapsedTime < rollDuration)
        {
            rb.linearVelocity = new Vector2(direction * rollSpeed, 0);
            elapsedTime += Time.deltaTime;

            // 구르기 시 무적
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = currentGravity;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        isRolling = false;
    }

    public void StepSound()
    {
        SoundManager.instance.PlaySFX(SFXType.StepSound);
    }



}
