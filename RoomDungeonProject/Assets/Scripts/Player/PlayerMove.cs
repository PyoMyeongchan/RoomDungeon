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
        

        // �ش� ��ġ�� ���̾ Ȯ���Ѵ�
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);
        
        // ���� ó��
        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
            moveSpeed = 3.0f;
        }
        
        if (moveInput != 0)
        {
            // �¿������ ����
            GetComponent<SpriteRenderer>().flipX = moveInput < 0;
        }

        // �޸��� ����
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


        //rigidbody2D���� position X,Y üũ�ϸ� ������ �ȵȴ�.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.SetJumping(isGrounded);
        }

        if (!isGrounded)
        {
            playerAnimation.SetFalling(!isGrounded);  
        }

        /* ���콺 �������� �̵� ���ϱ�
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimation.TriggerRolling();
            
            
        } 
        */

    }

}
