using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    
    void Start()
    {
        // Setup button listeners
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        if (backButton != null)
            backButton.onClick.AddListener(BackToMainMenu);
        
        // Show main menu, hide options
        ShowMainMenu();
    }
    
    public void PlayGame()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene(gameSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
    
    public void OpenOptions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }
    
    public void BackToMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }
}