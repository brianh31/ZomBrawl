using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Zombie Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int pointValue = 10;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    
    private int currentHealth;
    private Transform player;
    private Rigidbody2D rb;
    private float nextAttackTime;
    
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        // Check if close enough to attack
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    
    void FixedUpdate()
    {
        if (player == null) return;
        
        // Move toward player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        
        // Rotate to face player (adjusted by 180 degrees to face correct direction)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle;
    }
    
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Award points to player
        GameManager.Instance.AddPoints(pointValue);
        
        // Destroy zombie
        Destroy(gameObject);
    }
    
    void AttackPlayer()
    {
        // Get player health component and damage it
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
    
    // Make this public so we can set stats for different zombie types
    public void SetStats(int health, float speed, int dmg, int points)
    {
        maxHealth = health;
        currentHealth = health;
        moveSpeed = speed;
        damage = dmg;
        pointValue = points;
    }
}