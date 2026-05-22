using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float edgeCheckDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    int direction = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        Vector2 edgeOrigin = new Vector2(transform.position.x + direction * 0.5f, transform.position.y);
        bool groundAhead = Physics2D.Raycast(edgeOrigin, Vector2.down, edgeCheckDistance + 0.5f, groundLayer);
        bool wallAhead   = Physics2D.Raycast(transform.position, Vector2.right * direction, 0.4f, groundLayer);

        if (!groundAhead || wallAhead)
            Flip();
    }

    void Flip()
    {
        direction = -direction;
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
            Flip();
    }
}
