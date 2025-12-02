using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    [Header("Armor Settings")]
    [SerializeField] private int armorLevel = 0; // 0, 1, 2, or 3
    [SerializeField] private float[] armorReduction = { 0f, 0.1f, 0.25f, 0.4f }; // Damage reduction per level
    
    void Start()
    {
        currentHealth = maxHealth;
        
        // Update UI on start
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
        }
    }
    
    public void TakeDamage(int damage)
    {
        // Apply armor reduction
        float reduction = armorReduction[Mathf.Clamp(armorLevel, 0, 3)];
        int finalDamage = Mathf.RoundToInt(damage * (1 - reduction));
        
        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
        }
    }
    
    public void UpgradeArmor()
    {
        if (armorLevel < 3)
        {
            armorLevel++;
        }
    }
    
    public int GetArmorLevel()
    {
        return armorLevel;
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    void Die()
    {
        // Show game over screen
        if (GameOverManager.Instance != null && GameManager.Instance != null)
        {
            int finalScore = GameManager.Instance.GetCurrentPoints();
            int finalWave = 1; // Default wave
            
            // Try to get current wave from ZombieSpawner
            ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();
            if (spawner != null)
            {
                finalWave = spawner.GetCurrentWave();
            }
            
            GameOverManager.Instance.ShowGameOver(finalScore, finalWave);
        }
        else
        {
            // Fallback: just reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}