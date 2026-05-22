using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KamekTroll : MonoBehaviour
{
    [SerializeField] float minDelay = 10f;
    [SerializeField] float maxDelay = 30f;
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject kamekPanel;
    [SerializeField] Image progressBar;
    [SerializeField] AudioClip kamekSFX;

    PlayerController player;
    float timer;
    float nextActivation;
    bool active;

    void Start()
    {
        ScheduleNext();
        FindPlayer();
        if (kamekPanel != null) kamekPanel.SetActive(false);
    }

    void Update()
    {
        if (active) return;
        timer += Time.deltaTime;
        if (timer >= nextActivation)
            StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        active = true;
        if (player == null) FindPlayer();

        if (player != null) player.isControlsInverted = true;
        if (kamekPanel != null) kamekPanel.SetActive(true);
        AudioManager.Instance?.PlaySFX(kamekSFX);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (progressBar != null)
                progressBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        if (player != null) player.isControlsInverted = false;
        if (kamekPanel != null) kamekPanel.SetActive(false);

        timer = 0f;
        active = false;
        ScheduleNext();
    }

    public void ForceActivate()
    {
        if (!active) StartCoroutine(Activate());
    }

    void ScheduleNext()
    {
        nextActivation = Random.Range(minDelay, maxDelay);
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<PlayerController>();
    }
}
