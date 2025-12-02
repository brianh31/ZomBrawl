using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    [Header("Weapon System")]
    [SerializeField] private SpriteRenderer characterSpriteRenderer; // The main character sprite renderer
    [SerializeField] private List<WeaponData> ownedWeapons = new List<WeaponData>();
    [SerializeField] private int currentWeaponIndex = 0;
    
    [Header("Audio")]
    [SerializeField] private float gunSoundVolume = 0.7f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private Camera mainCamera;
    private float nextFireTime;
    private AudioSource audioSource;
    
    private WeaponData currentWeapon;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        // Get or create audio source for gun sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = gunSoundVolume;
        audioSource.playOnAwake = false;
        
        // Set initial weapon if we have any
        if (ownedWeapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }
    
    void Update()
    {
        // Get movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Get mouse position in world space
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Weapon switching with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1) && ownedWeapons.Count >= 1)
        {
            EquipWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && ownedWeapons.Count >= 2)
        {
            EquipWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && ownedWeapons.Count >= 3)
        {
            EquipWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && ownedWeapons.Count >= 4)
        {
            EquipWeapon(3);
        }
        
        // Shooting
        if (currentWeapon != null)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + currentWeapon.fireRate;
            }
        }
    }
    
    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
        
        // Rotate player to face mouse
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    
    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || currentWeapon == null) return;
        
        // Play gun sound
        if (currentWeapon.gunSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(currentWeapon.gunSound, gunSoundVolume);
        }
        
        // Shoot multiple bullets for shotgun
        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            // Calculate spread angle
            float spreadOffset = 0f;
            if (currentWeapon.bulletsPerShot > 1)
            {
                spreadOffset = Random.Range(-currentWeapon.spreadAngle, currentWeapon.spreadAngle);
            }
            
            // Create bullet with adjusted rotation
            Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, 90f + spreadOffset);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            
            // Set bullet damage and speed
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(currentWeapon.damage);
                bulletScript.SetSpeed(currentWeapon.bulletSpeed);
            }
        }
    }
    
    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= ownedWeapons.Count) return;
        
        currentWeaponIndex = index;
        currentWeapon = ownedWeapons[index];
        
        // Update character sprite to show weapon
        if (characterSpriteRenderer != null && currentWeapon.weaponSprite != null)
        {
            characterSpriteRenderer.sprite = currentWeapon.weaponSprite;
        }
        
        Debug.Log($"Equipped: {currentWeapon.weaponName}");
    }
    
    public void AddWeapon(WeaponData weapon)
    {
        // Check if we already own this weapon
        if (!ownedWeapons.Contains(weapon))
        {
            ownedWeapons.Add(weapon);
            
            // Auto-equip if it's our first weapon
            if (ownedWeapons.Count == 1)
            {
                EquipWeapon(0);
            }
            
            Debug.Log($"Added weapon: {weapon.weaponName}");
        }
    }
    
    public bool HasWeapon(WeaponData weapon)
    {
        return ownedWeapons.Contains(weapon);
    }
    
    public List<WeaponData> GetOwnedWeapons()
    {
        return ownedWeapons;
    }
}