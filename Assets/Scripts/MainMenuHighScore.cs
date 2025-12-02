using UnityEngine;
using UnityEngine.UI;

public class MainMenuHighScore : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private Text highestWaveText;
    [SerializeField] private Text highestScoreText;
    
    void Start()
    {
        // Wait a frame to ensure HighScoreManager is loaded
        Invoke("UpdateHighScoreDisplay", 0.1f);
    }
    
    void UpdateHighScoreDisplay()
    {
        if (HighScoreManager.Instance != null)
        {
            int highWave = HighScoreManager.Instance.GetHighestWave();
            int highScore = HighScoreManager.Instance.GetHighestScore();
            
            if (highestWaveText != null)
            {
                highestWaveText.text = $"Best Wave: {highWave}";
            }
            
            if (highestScoreText != null)
            {
                highestScoreText.text = $"High Score: {highScore}";
            }
        }
    }
}