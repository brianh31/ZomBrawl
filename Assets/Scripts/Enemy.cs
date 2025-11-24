using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 30;
    public float speed = 2.5f;
    public int touchDamage = 10;
    public int coinDrop = 10;

    [Header("Facing")]
    // If the sprite looks turned, try -90, +90, or 0
    public float spriteForwardOffset = -90f;

    // Spawner listens for this to know when all enemies are dead
    public static System.Action OnAnyEnemyDied;

    int currentHealth;
    Rigidbody2D rb;
    Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // ensure correct tag for any tag-based logic elsewhere
        gameObject.tag = "Enemy";
    }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (!player || !rb) return;

        // Move toward player
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;  // (Unity 6) falls back to .velocity if needed

        // Face movement direction
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + spriteForwardOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, ang);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        GameController.Instance?.AddCoins(coinDrop);
        OnAnyEnemyDied?.Invoke();      // notify spawner
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Safety: if destroyed after hitting 0 HP via some other script
        if (currentHealth <= 0)
            OnAnyEnemyDied?.Invoke();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.CompareTag("Player"))
        {
            if (c.collider.TryGetComponent<PlayerHealth>(out var ph))
                ph.TakeDamage(touchDamage);
        }
    }
}
