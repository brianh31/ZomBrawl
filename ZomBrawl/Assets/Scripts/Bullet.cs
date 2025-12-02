using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifetime = 3f;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Use transform.right instead of transform.up for horizontal bullet sprites
        rb.linearVelocity = transform.right * speed;
        
        // Destroy bullet after lifetime expires
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if bullet hit a zombie
        if (collision.CompareTag("Zombie"))
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        
        // Destroy bullet if it hits a wall
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    public int GetDamage()
    {
        return damage;
    }
    
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
    }
}