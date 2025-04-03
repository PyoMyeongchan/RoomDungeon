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

        // �ش� ��ġ�� ���̾ Ȯ���Ѵ�
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);
        
        // ���� ó��
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


        // ��������
        //rigidbody2D���� position X,Y üũ�ϸ� ������ �ȵȴ�.
        if (Input.GetButtonDown("Jump") && isGrounded && !isRolling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.TriggerJumping();
            SoundManager.instance.PlaySFX(SFXType.JumpSound);

        }

        // ���ϻ���
        if (!isGrounded)
        {
            playerAnimation.SetFalling(true);

        }
        else if (isGrounded)
        {            
            playerAnimation.SetFalling(false);

            airRolling = true;
        }

        

        // ������
        if (Input.GetMouseButtonDown(1) && !isRolling && playerAttack.isAttacking == false)
        {
            if (isGrounded || airRolling)
            {
                Rolling();

                // ������ ������ �ѹ��� �����ϰ� �ϱ�
                if (!isGrounded)
                {
                    airRolling = false;
                }
            }
        }

        // ������� �ٸ� ���� ���ϵ��� ����
        if (isRolling)
        {
            return;
        }
        

    }

    // ���콺�����ͷ� �¿� ����
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

            // ������ �� ����
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
