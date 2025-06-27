using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int currentLevel = 1;
    private int currentScore = 0;
    private const string LEVEL_KEY = "CurrentLevel";
    private const string SCORE_KEY = "CurrentScore";

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData();
    }

    // Load dữ liệu từ PlayerPrefs
    public void LoadData()
    {
        currentLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
        currentScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    // Lưu dữ liệu vào PlayerPrefs
    public void SaveData()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, currentLevel);
        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
    }

    // Load level cụ thể
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 1 && levelIndex <= SceneManager.sceneCountInBuildSettings)
        {
            currentLevel = levelIndex;
            SaveData();
            SceneManager.LoadScene(levelIndex - 1); // Scene index bắt đầu từ 0
        }
        else
        {
            Debug.LogError("Invalid level index: " + levelIndex);
        }
    }

    // Chuyển sang level tiếp theo
    public void NextLevel()
    {
        int nextLevel = currentLevel + 1;
        if (nextLevel <= SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            Debug.LogWarning("No more levels available!");
            // Có thể thêm logic khi hoàn thành tất cả level
        }
    }

    public void ReloadLevel()
    {
        LoadLevel(currentLevel);
    }

    // Reset về level đầu tiên
    public void ResetToFirstLevel()
    {
        currentLevel = 1;
        currentScore = 0;
        SaveData();
        LoadLevel(1);
    }

    // Getter và Setter cho level và score
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
        SaveData();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        SaveData();
    }

    public void ResetScore()
    {
        currentScore = 0;
        SaveData();
    }
}