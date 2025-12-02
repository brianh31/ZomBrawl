using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("UI Elements")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text pointsText;
    [SerializeField] private Text waveText;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("UIManager Instance created");
        }
        else
        {
            Debug.LogWarning("Multiple UIManagers detected, destroying duplicate");
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Initialize displays with default values
        InitializeUI();
    }
    
    void InitializeUI()
    {
        // Find player and initialize health
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                UpdateHealthUI(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
            }
        }
        
        // Initialize points
        if (GameManager.Instance != null)
        {
            UpdatePointsUI(GameManager.Instance.GetCurrentPoints());
        }
        
        // Initialize wave
        UpdateWaveUI(1);
    }
    
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }
    
    public void UpdatePointsUI(int points)
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {points}";
        }
    }
    
    public void UpdateWaveUI(int wave)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {wave}";
        }
    }
}