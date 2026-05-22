using System.Collections;
using TMPro;
using UnityEngine;

public class FakeWin : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] AudioClip winJingle;
    [SerializeField] float fakeDelay = 2f;

    public void Trigger()
    {
        StartCoroutine(FakeSequence());
    }

    IEnumerator FakeSequence()
    {
        if (winPanel != null) winPanel.SetActive(true);
        if (winText  != null) winText.text = "YOU WIN \U0001f389";
        AudioManager.Instance?.PlaySFX(winJingle);

        yield return new WaitForSeconds(fakeDelay);

        if (winText != null) winText.text = "lol non";

        yield return new WaitForSeconds(1f);

        if (winPanel != null) winPanel.SetActive(false);

        WaveManager wm = FindAnyObjectByType<WaveManager>();
        if (wm != null)
            wm.StartWaves(OnAllWavesDone);
        else
            GameManager.Instance.LoadNextLevel();
    }

    void OnAllWavesDone()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
