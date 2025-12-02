using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Stats")]
    [SerializeField] private int currentPoints = 0;
    
    void Awake()
    {
        // Singleton pattern
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
        // Initialize UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePointsUI(currentPoints);
        }
    }
    
    public void AddPoints(int points)
    {
        currentPoints += points;
        
        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePointsUI(currentPoints);
        }
    }
    
    public bool SpendPoints(int cost)
    {
        if (currentPoints >= cost)
        {
            currentPoints -= cost;
            
            // Update UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdatePointsUI(currentPoints);
            }
            
            return true;
        }
        
        return false;
    }
    
    public int GetCurrentPoints()
    {
        return currentPoints;
    }
}