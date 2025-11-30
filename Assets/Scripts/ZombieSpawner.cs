using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    [SerializeField] private GameObject normalZombiePrefab;
    [SerializeField] private GameObject fastZombiePrefab;
    [SerializeField] private GameObject tankZombiePrefab;
    [SerializeField] private GameObject bossZombiePrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDistance = 15f; // Distance from player to spawn
    [SerializeField] private int initialWaveSize = 3;
    [SerializeField] private float waveIncreaseFactor = 1.2f;
    
    [Header("Wave Settings")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private bool spawnBoss = false;
    [SerializeField] private int bossWave = 5; // Boss appears every X waves
    [SerializeField] private float timeBetweenWaves = 5f; // Time to wait after all zombies are killed
    
    private Transform player;
    private int zombiesThisWave;
    private int zombiesSpawned;
    
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        StartCoroutine(SpawnWaves());
    }
    
    IEnumerator SpawnWaves()
    {
        while (true)
        {
            currentWave++;
            zombiesThisWave = Mathf.RoundToInt(initialWaveSize * Mathf.Pow(waveIncreaseFactor, currentWave - 1));
            zombiesSpawned = 0;
            
            Debug.Log($"Wave {currentWave} starting! Zombies: {zombiesThisWave}");
            
            // Update wave UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateWaveUI(currentWave);
            }
            
            // Check if it's a boss wave
            if (currentWave % bossWave == 0)
            {
                SpawnBoss();
                yield return new WaitForSeconds(spawnInterval * 2);
            }
            
            // Spawn regular zombies
            while (zombiesSpawned < zombiesThisWave)
            {
                SpawnZombie();
                zombiesSpawned++;
                yield return new WaitForSeconds(spawnInterval);
            }
            
            // Wait until all zombies are killed
            Debug.Log("All zombies spawned. Waiting for them to be killed...");
            yield return new WaitUntil(() => AllZombiesKilled());
            
            Debug.Log("All zombies killed! Next wave starting soon...");
            
            // Short delay before next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
    
    bool AllZombiesKilled()
    {
        // Count all zombies currently in the scene
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        return zombies.Length == 0;
    }
    
    void SpawnZombie()
    {
        if (player == null) return;
        
        // Random position around player
        Vector2 spawnPosition = GetRandomSpawnPosition();
        
        // Determine zombie type based on wave number
        GameObject zombiePrefab = GetRandomZombieType();
        
        if (zombiePrefab != null)
        {
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    void SpawnBoss()
    {
        if (player == null || bossZombiePrefab == null) return;
        
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(bossZombiePrefab, spawnPosition, Quaternion.identity);
        
        Debug.Log("BOSS SPAWNED!");
    }
    
    Vector2 GetRandomSpawnPosition()
    {
        // Spawn at random angle around player at fixed distance
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        return (Vector2)player.position + direction * spawnDistance;
    }
    
    GameObject GetRandomZombieType()
    {
        // Gradually introduce harder zombies as waves progress
        float rand = Random.value;
        
        if (currentWave < 3)
        {
            // Early waves: only normal zombies
            return normalZombiePrefab;
        }
        else if (currentWave < 6)
        {
            // Mid waves: normal and fast
            return rand < 0.7f ? normalZombiePrefab : fastZombiePrefab;
        }
        else
        {
            // Late waves: all types
            if (rand < 0.5f)
                return normalZombiePrefab;
            else if (rand < 0.8f)
                return fastZombiePrefab;
            else
                return tankZombiePrefab;
        }
    }
    
    public int GetCurrentWave()
    {
        return currentWave;
    }
}