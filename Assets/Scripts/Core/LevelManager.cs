using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] string[] levelSceneNames = { "Level1", "Level2", "Level3", "Level4" };

    public int CurrentLevelIndex { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            if (scene.name == levelSceneNames[i])
            {
                CurrentLevelIndex = i;
                return;
            }
        }
    }

    public void LoadNextLevel()
    {
        int next = CurrentLevelIndex + 1;
        if (next < levelSceneNames.Length)
            SceneManager.LoadScene(levelSceneNames[next]);
        else
            SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelSceneNames.Length) return;
        CurrentLevelIndex = index;
        SceneManager.LoadScene(levelSceneNames[index]);
    }

    public void RestartFromBeginning()
    {
        LoadLevel(0);
    }

    public void SaveStarForLevel(int levelIndex)
    {
        PlayerPrefs.SetInt($"Star_{levelIndex}", 1);
        PlayerPrefs.Save();
    }

    public bool HasStarForLevel(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Star_{levelIndex}", 0) == 1;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
