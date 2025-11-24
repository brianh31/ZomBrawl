using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;

    [Header("Wave Size")]
    public int baseCount = 6;
    public int perWaveIncrement = 3;

    [Header("Timing")]
    public float timeBetweenWaves = 1.0f;

    [Header("Spawn Placement")]
    public float spawnPadding = 1.5f;   // how far off-screen to spawn
    public bool spawnInsideForDebug = false;

    int currentWave = 0;
    int enemiesAlive = 0;
    bool spawning = false;

    void OnEnable()  { Enemy.OnAnyEnemyDied += HandleEnemyDied; }
    void OnDisable() { Enemy.OnAnyEnemyDied -= HandleEnemyDied; }

    void Start()
    {
        // kick off first wave
        StartCoroutine(NextWaveDelay());
    }

    void HandleEnemyDied()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        if (!spawning && enemiesAlive == 0)
            StartCoroutine(NextWaveDelay());
    }

    IEnumerator NextWaveDelay()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        SpawnWave();
    }

    void SpawnWave()
    {
        if (spawning) return;
        spawning = true;

        currentWave++;
        if (UIManager.Instance != null) UIManager.Instance.UpdateWave(currentWave);

        int toSpawn = baseCount + (currentWave - 1) * perWaveIncrement;

        for (int i = 0; i < toSpawn; i++)
        {
            Vector3 pos = spawnInsideForDebug ? RandomSpawnInsideCamera(0.8f) : RandomOffscreenSpawn();
            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemiesAlive++;

            // gentle scaling per wave
            var enemy = go.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.maxHealth = Mathf.RoundToInt(enemy.maxHealth + currentWave * 2);
                enemy.speed += Mathf.Min(0.02f * currentWave, 1.5f);
            }

            var rb = go.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = 0f;
        }

        spawning = false;
    }

    Vector3 RandomOffscreenSpawn()
    {
        Camera cam = Camera.main;
        if (!cam) return transform.position;

        float h = cam.orthographicSize;
        float w = h * cam.aspect;
        Vector2 center = cam.transform.position;

        int side = Random.Range(0, 4);  // 0 left, 1 right, 2 top, 3 bottom
        switch (side)
        {
            case 0: return new Vector3(center.x - w - spawnPadding, Random.Range(center.y - h, center.y + h), 0f);
            case 1: return new Vector3(center.x + w + spawnPadding, Random.Range(center.y - h, center.y + h), 0f);
            case 2: return new Vector3(Random.Range(center.x - w, center.x + w), center.y + h + spawnPadding, 0f);
            default: return new Vector3(Random.Range(center.x - w, center.x + w), center.y - h - spawnPadding, 0f);
        }
    }

    Vector3 RandomSpawnInsideCamera(float marginPercent = 0.9f)
    {
        Camera cam = Camera.main;
        if (!cam) return transform.position;

        float h = cam.orthographicSize * marginPercent;
        float w = h * cam.aspect;
        Vector2 center = cam.transform.position;

        return new Vector3(Random.Range(center.x - w, center.x + w),
                           Random.Range(center.y - h, center.y + h), 0f);
    }
}
