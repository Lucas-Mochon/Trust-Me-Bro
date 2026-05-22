using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float detectionRange = 6f;

    Rigidbody2D rb;
    Transform player;
    bool chasing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }

        float dist = Mathf.Abs(player.position.x - transform.position.x);
        chasing = dist <= detectionRange;
    }

    void FixedUpdate()
    {
        if (!chasing || player == null) { rb.linearVelocity = Vector2.zero; return; }

        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        transform.localScale = new Vector3(dir, 1, 1);
    }

}
