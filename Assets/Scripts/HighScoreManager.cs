using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    
    private int highestWave = 0;
    private int highestScore = 0;
    
    void Awake()
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
            return;
        }
        
        // Load saved high scores
        LoadHighScores();
    }
    
    void LoadHighScores()
    {
        // Load from PlayerPrefs (Unity's built-in save system)
        highestWave = PlayerPrefs.GetInt("HighestWave", 0);
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        
        Debug.Log($"Loaded High Scores - Wave: {highestWave}, Score: {highestScore}");
    }
    
    public void CheckAndSaveHighScores(int currentWave, int currentScore)
    {
        bool updated = false;
        
        // Check if current wave is higher
        if (currentWave > highestWave)
        {
            highestWave = currentWave;
            PlayerPrefs.SetInt("HighestWave", highestWave);
            updated = true;
            Debug.Log($"New highest wave: {highestWave}");
        }
        
        // Check if current score is higher
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetInt("HighestScore", highestScore);
            updated = true;
            Debug.Log($"New highest score: {highestScore}");
        }
        
        // Save to disk
        if (updated)
        {
            PlayerPrefs.Save();
        }
    }
    
    public int GetHighestWave()
    {
        return highestWave;
    }
    
    public int GetHighestScore()
    {
        return highestScore;
    }
    
    // Optional: Reset high scores (for testing or options menu)
    public void ResetHighScores()
    {
        highestWave = 0;
        highestScore = 0;
        PlayerPrefs.SetInt("HighestWave", 0);
        PlayerPrefs.SetInt("HighestScore", 0);
        PlayerPrefs.Save();
        Debug.Log("High scores reset!");
    }
}