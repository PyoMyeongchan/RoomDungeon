using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float jumpForce = 6.0f;

    private Rigidbody2D rb;
    private Rigidbody2D rb2;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private PlayerAnimation playerAnimation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb2 = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
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
            moveSpeed = 3.0f;
        }

        // 좌우움직임 구현
        if (moveInput != 0)
        {
            
            GetComponent<SpriteRenderer>().flipX = moveInput < 0;
        }

        // 달리기 구현
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
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
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.TriggerJumping();

        }

        // 낙하상태
        if (!isGrounded)
        {
            playerAnimation.SetFalling(true);

        }
        else if (isGrounded)
        {            
            playerAnimation.SetFalling(false);
        }



        

        /* 마우스 방향으로 이동 롤하기
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimation.TriggerRolling();
            
            
        } 
        */

    }

}
