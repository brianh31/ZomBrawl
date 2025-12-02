using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    
    [Header("Weapon Data")]
    [SerializeField] private WeaponData pistolData;
    [SerializeField] private WeaponData rifleData;
    [SerializeField] private WeaponData shotgunData;
    [SerializeField] private WeaponData smgData;
    
    [Header("Shop Buttons")]
    [SerializeField] private Button rifleButton;
    [SerializeField] private Button shotgunButton;
    [SerializeField] private Button smgButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button armorButton;
    
    [Header("Button Text")]
    [SerializeField] private Text rifleButtonText;
    [SerializeField] private Text shotgunButtonText;
    [SerializeField] private Text smgButtonText;
    [SerializeField] private Text healButtonText;
    [SerializeField] private Text armorButtonText;
    
    [Header("Shop Settings")]
    [SerializeField] private int healAmount = 50;
    [SerializeField] private int healCost = 50;
    [SerializeField] private int[] armorCosts = { 100, 200, 300 }; // Level 1, 2, 3
    
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ShopManager Instance created");
        }
        else
        {
            Debug.LogWarning("Multiple ShopManagers detected, destroying duplicate");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Verify all weapon data is assigned
        if (pistolData == null) Debug.LogError("ShopManager: PistolData not assigned!");
        if (rifleData == null) Debug.LogError("ShopManager: RifleData not assigned!");
        if (shotgunData == null) Debug.LogError("ShopManager: ShotgunData not assigned!");
        if (smgData == null) Debug.LogError("ShopManager: SMGData not assigned!");
        
        // Verify all buttons are assigned
        if (rifleButton == null) Debug.LogError("ShopManager: RifleButton not assigned!");
        if (shotgunButton == null) Debug.LogError("ShopManager: ShotgunButton not assigned!");
        if (smgButton == null) Debug.LogError("ShopManager: SMGButton not assigned!");
        if (healButton == null) Debug.LogError("ShopManager: HealButton not assigned!");
        if (armorButton == null) Debug.LogError("ShopManager: ArmorButton not assigned!");
        
        // Verify all button texts are assigned
        if (rifleButtonText == null) Debug.LogError("ShopManager: RifleButtonText not assigned!");
        if (shotgunButtonText == null) Debug.LogError("ShopManager: ShotgunButtonText not assigned!");
        if (smgButtonText == null) Debug.LogError("ShopManager: SMGButtonText not assigned!");
        if (healButtonText == null) Debug.LogError("ShopManager: HealButtonText not assigned!");
        if (armorButtonText == null) Debug.LogError("ShopManager: ArmorButtonText not assigned!");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerHealth = player.GetComponent<PlayerHealth>();
            
            if (playerController == null) Debug.LogError("PlayerController not found on Player!");
            if (playerHealth == null) Debug.LogError("PlayerHealth not found on Player!");
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        
        // Give player starting pistol
        if (playerController != null && pistolData != null)
        {
            playerController.AddWeapon(pistolData);
            Debug.Log("Starting pistol added to player");
        }
        
        // Setup button listeners
        if (rifleButton != null)
            rifleButton.onClick.AddListener(BuyRifle);
        if (shotgunButton != null)
            shotgunButton.onClick.AddListener(BuyShotgun);
        if (smgButton != null)
            smgButton.onClick.AddListener(BuySMG);
        if (healButton != null)
            healButton.onClick.AddListener(BuyHeal);
        if (armorButton != null)
            armorButton.onClick.AddListener(BuyArmor);
        
        // Set initial button texts
        UpdateButtonTexts();
        
        // Update button texts every 0.5 seconds
        InvokeRepeating("UpdateButtonTexts", 0.5f, 0.5f);
    }
    
    void OnDestroy()
    {
        CancelInvoke();
    }
    
    void BuyRifle()
    {
        if (rifleData == null || playerController == null) return;
        
        if (playerController.HasWeapon(rifleData))
        {
            Debug.Log("Already own Rifle!");
            return;
        }
        
        if (GameManager.Instance != null && GameManager.Instance.SpendPoints(rifleData.cost))
        {
            playerController.AddWeapon(rifleData);
            Debug.Log("Purchased Rifle!");
            UpdateButtonTexts();
        }
        else
        {
            Debug.Log("Not enough points for Rifle!");
        }
    }
    
    void BuyShotgun()
    {
        if (shotgunData == null || playerController == null) return;
        
        if (playerController.HasWeapon(shotgunData))
        {
            Debug.Log("Already own Shotgun!");
            return;
        }
        
        if (GameManager.Instance != null && GameManager.Instance.SpendPoints(shotgunData.cost))
        {
            playerController.AddWeapon(shotgunData);
            Debug.Log("Purchased Shotgun!");
            UpdateButtonTexts();
        }
        else
        {
            Debug.Log("Not enough points for Shotgun!");
        }
    }
    
    void BuySMG()
    {
        if (smgData == null || playerController == null) return;
        
        if (playerController.HasWeapon(smgData))
        {
            Debug.Log("Already own SMG!");
            return;
        }
        
        if (GameManager.Instance != null && GameManager.Instance.SpendPoints(smgData.cost))
        {
            playerController.AddWeapon(smgData);
            Debug.Log("Purchased SMG!");
            UpdateButtonTexts();
        }
        else
        {
            Debug.Log("Not enough points for SMG!");
        }
    }
    
    void BuyHeal()
    {
        if (playerHealth == null) return;
        
        if (playerHealth.GetCurrentHealth() >= playerHealth.GetMaxHealth())
        {
            Debug.Log("Already at full health!");
            return;
        }
        
        if (GameManager.Instance != null && GameManager.Instance.SpendPoints(healCost))
        {
            playerHealth.Heal(healAmount);
            Debug.Log("Healed!");
        }
        else
        {
            Debug.Log("Not enough points for Heal!");
        }
    }
    
    void BuyArmor()
    {
        if (playerHealth == null) return;
        
        int currentArmor = playerHealth.GetArmorLevel();
        
        if (currentArmor >= 3)
        {
            Debug.Log("Max armor level reached!");
            return;
        }
        
        int cost = armorCosts[currentArmor];
        
        if (GameManager.Instance != null && GameManager.Instance.SpendPoints(cost))
        {
            playerHealth.UpgradeArmor();
            Debug.Log($"Upgraded to Armor Level {currentArmor + 1}!");
            UpdateButtonTexts();
        }
        else
        {
            Debug.Log("Not enough points for Armor!");
        }
    }
    
    void UpdateButtonTexts()
    {
        if (playerController == null || playerHealth == null) return;
        
        // Rifle button
        if (rifleButtonText != null && rifleData != null)
        {
            if (playerController.HasWeapon(rifleData))
            {
                rifleButtonText.text = "OWNED";
            }
            else
            {
                rifleButtonText.text = "Rifle $" + rifleData.cost;
            }
        }
        
        // Shotgun button
        if (shotgunButtonText != null && shotgunData != null)
        {
            if (playerController.HasWeapon(shotgunData))
            {
                shotgunButtonText.text = "OWNED";
            }
            else
            {
                shotgunButtonText.text = "Shotgun $" + shotgunData.cost;
            }
        }
        
        // SMG button
        if (smgButtonText != null && smgData != null)
        {
            if (playerController.HasWeapon(smgData))
            {
                smgButtonText.text = "OWNED";
            }
            else
            {
                smgButtonText.text = "SMG $" + smgData.cost;
            }
        }
        
        // Heal button
        if (healButtonText != null)
        {
            healButtonText.text = "Heal $" + healCost;
        }
        
        // Armor button
        if (armorButtonText != null)
        {
            int armorLevel = playerHealth.GetArmorLevel();
            if (armorLevel >= 3)
            {
                armorButtonText.text = "MAX ARMOR";
            }
            else
            {
                armorButtonText.text = "Armor Lv" + (armorLevel + 1) + " $" + armorCosts[armorLevel];
            }
        }
    }
}