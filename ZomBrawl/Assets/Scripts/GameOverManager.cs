using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("UI Elements")]
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text finalWaveText;
    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // Hide game over panel at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
    
    public void ShowGameOver(int finalScore, int finalWave)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        // Update final stats
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + finalScore;
        
        if (finalWaveText != null)
            finalWaveText.text = "Wave Reached: " + finalWave;
        
        // Freeze the game
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1f; // Resume time
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}