using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public int cost;
    public Sprite weaponSprite; // The sprite to display on the player
    
    [Header("Weapon Stats")]
    public float fireRate;
    public int damage;
    public float bulletSpeed;
    public int bulletsPerShot = 1; // For shotgun spreading
    public float spreadAngle = 0f; // Shotgun spread
    
    [Header("Audio")]
    public AudioClip gunSound; // Sound effect for this weapon
    
    [Header("Ammo (Optional)")]
    public bool hasInfiniteAmmo = true;
    public int magazineSize = 30;
    public float reloadTime = 2f;
}