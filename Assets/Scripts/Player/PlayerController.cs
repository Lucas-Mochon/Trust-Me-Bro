using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float stompBounce = 8f;

    public bool isControlsInverted;

    Rigidbody2D rb;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (isControlsInverted) input = -input;

        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float jumpInput = isControlsInverted ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpInput);
        }

        if (input != 0)
            transform.localScale = new Vector3(input > 0 ? 1 : -1, 1, 1);
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance + 0.5f, groundLayer);
        isGrounded = hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;

        if (col.gameObject.GetComponent<EnemyShooter>() != null)
        {
            GameManager.Instance.LoseLife();
            return;
        }

        ContactPoint2D contact = col.GetContact(0);
        bool stompedFromAbove = rb.linearVelocity.y < -0.1f && contact.normal.y > 0.5f;

        if (stompedFromAbove)
        {
            Destroy(col.gameObject);
            GameManager.Instance.AddScore(100);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stompBounce);
        }
        else
        {
            GameManager.Instance.LoseLife();
        }
    }
}
