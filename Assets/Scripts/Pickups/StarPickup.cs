using UnityEngine;

public class StarPickup : MonoBehaviour
{
    [SerializeField] AudioClip collectSFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance?.CollectStar();
        if (LevelManager.Instance != null)
            LevelManager.Instance.SaveStarForLevel(LevelManager.Instance.CurrentLevelIndex);
        AudioManager.Instance?.PlaySFX(collectSFX);
        Destroy(gameObject);
    }
}
