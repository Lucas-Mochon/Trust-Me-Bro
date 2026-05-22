using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI trollMessageText;

    float timer;
    bool counting = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (!counting) return;
        timer += Time.deltaTime;
        if (timerText != null)
            timerText.text = FormatTime(timer);
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null) livesText.text = "x" + lives;
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString("D6");
    }

    public void ShowTrollMessage(string msg, float duration = 2f)
    {
        if (trollMessageText == null) return;
        StopCoroutine(nameof(HideTrollMessage));
        trollMessageText.text = msg;
        trollMessageText.gameObject.SetActive(true);
        StartCoroutine(HideTrollMessage(duration));
    }

    IEnumerator HideTrollMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trollMessageText != null) trollMessageText.gameObject.SetActive(false);
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60);
        int s = (int)(t % 60);
        return $"{m:00}:{s:00}";
    }
}
