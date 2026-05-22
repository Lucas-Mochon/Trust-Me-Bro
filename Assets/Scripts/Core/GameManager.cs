using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int startingLives = 4;

    public int Lives { get; private set; }
    public int Score { get; private set; }
    public int StarsCollected { get; private set; }

    Vector3 checkpointPosition;
    bool hasCheckpoint;
    GameObject player;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Lives = startingLives;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hasCheckpoint = false;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UIManager.Instance?.UpdateScore(Score);
    }

    public void CollectStar()
    {
        StarsCollected++;
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        hasCheckpoint = true;
    }

    public void LoseLife()
    {
        Lives--;
        UIManager.Instance?.UpdateLives(Lives);

        if (Lives <= 0) { GameOver(); return; }

        if (player != null)
        {
            Vector3 respawn = hasCheckpoint ? checkpointPosition : Vector3.zero;
            player.transform.position = respawn;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void WinLevel()
    {
        FakeWin fw = FindAnyObjectByType<FakeWin>();
        if (fw != null)
            fw.Trigger();
        else
            LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        if (LevelManager.Instance != null) { LevelManager.Instance.LoadNextLevel(); return; }

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
