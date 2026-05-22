using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] float fireRate = 2.5f;
    [SerializeField] float detectionRange = 8f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    Transform player;
    float nextFireTime;

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }

        float dist = Mathf.Abs(player.position.x - transform.position.x);
        if (dist > detectionRange) return;

        float dir = Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = new Vector3(dir, 1, 1);

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot(dir);
        }
    }

    void Shoot(float dir)
    {
        if (projectilePrefab == null) return;
        Transform origin = firePoint != null ? firePoint : transform;
        GameObject proj = Instantiate(projectilePrefab, origin.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null) p.SetDirection(dir);
    }

    // Ne peut pas être stompé
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            GameManager.Instance.LoseLife();
    }
}
