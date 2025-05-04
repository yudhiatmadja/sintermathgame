using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 15f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isGrounded;
    public Transform groundCheck; // Tambahkan empty object sebagai groundCheck
    public LayerMask groundLayer; // LayerMask untuk ground

    // Komponen audio
    

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        

        if (body == null) Debug.LogError("Rigidbody2D tidak ditemukan pada " + gameObject.name);
        if (anim == null) Debug.LogError("Animator tidak ditemukan pada " + gameObject.name);
        

        body.gravityScale = 3f; 

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        // if(isGrounded){
        //     Debug.Log("pemain di tanah");
        // }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);
        MovePlayer();
        JumpPlayer();
        ApplyGravityMultiplier();
        // Debug.Log("isGrounded: " + isGrounded);

    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Gerakan karakter
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Flip karakter
        if (horizontalInput > 0)
            sprite.flipX = false;
        else if (horizontalInput < 0)
            sprite.flipX = true;

        // Pastikan nilai Speed selalu lebih dari 0 agar Idle tetap berjalan
        // float animSpeed = Mathf.Abs(body.linearVelocity.x) > 0.01f ? Mathf.Abs(body.linearVelocity.x) : 0.01f;
        // anim.SetFloat("Speed", animSpeed);
        anim.SetFloat("Speed", Mathf.Abs(body.linearVelocity.x));
    }

    void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            anim.SetBool("IsJumping", true); 

            // Mainkan suara lompat jika ada
            if (Time.timeSinceLevelLoad > 0.1f)
            {
                AudioManager.instance.PlaySound("jump");
            }
        }
    }

    void ApplyGravityMultiplier()
    {
        if (body.linearVelocity.y < 0) 
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space)) 
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("IsJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
        }
    }
}
