using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
}

public class EnemySpawnTrigger : MonoBehaviour
{
    public Transform player;
    public List<SpawnInfo> spawners = new List<SpawnInfo>();

    // This will enforce one-by-one spawning. Set to 1 in the Inspector.
    public int maxAliveAtOnce = 1;
    public float timeBetweenSpawns = 0.7f;

    private bool started;
    private bool finished;

    private int spawnedCount;
    private float nextSpawnTime;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (finished || started) return;

        // The trigger starts when the player enters the volume
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.transform;
            started = true;
            nextSpawnTime = Time.time;
            Debug.Log("Spawn trigger sequence initiated.");
        }
    }

    void Update()
    {
        if (!started || finished) return;

        CleanupDeadEnemies();

        // If all enemies from the configured spawners have been spawned...
        if (spawnedCount >= spawners.Count)
        {
            // ...and all of them are now dead, the sequence is complete.
            if (aliveEnemies.Count == 0)
            {
                Debug.Log("All spawned enemies defeated. Sequence complete.");
                finished = true;
                // You can uncomment the line below to disable the trigger object when finished
                // gameObject.SetActive(false);
            }
            return;
        }

        if (Time.time < nextSpawnTime) return;

        // Only spawn if the number of alive enemies is less than the max allowed
        if (aliveEnemies.Count >= maxAliveAtOnce)
        {
            nextSpawnTime = Time.time + 0.1f; // Short delay to re-check soon
            return;
        }

        // If we've passed all checks, spawn the next enemy in the sequence
        SpawnNextEnemy();

        spawnedCount++;
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

    void SpawnNextEnemy()
    {
        if (spawnedCount >= spawners.Count) return;

        SpawnInfo currentSpawn = spawners[spawnedCount];
        if (currentSpawn.enemyPrefab == null || currentSpawn.spawnPoint == null)
        {
            Debug.LogError($"Spawner #{spawnedCount} in the list is not configured correctly (missing prefab or spawn point).");
            return;
        }

        Debug.Log($"Spawning enemy #{spawnedCount + 1} ({currentSpawn.enemyPrefab.name}) at {currentSpawn.spawnPoint.name}.");

        GameObject enemyObj = Instantiate(currentSpawn.enemyPrefab, currentSpawn.spawnPoint.position, currentSpawn.spawnPoint.rotation);
        aliveEnemies.Add(enemyObj);

        // This part finds the correct enemy script (Attack, FireSpit, etc.) and assigns the player as its target.
        EnemyMovement enemyMovement = enemyObj.GetComponent<EnemyMovement>();
        if (enemyMovement != null) enemyMovement.target = player;

        EnemyAttack enemyAttack = enemyObj.GetComponent<EnemyAttack>();
        if (enemyAttack != null) enemyAttack.target = player;

        EnemyFireSpit enemyFireSpit = enemyObj.GetComponent<EnemyFireSpit>();
        if (enemyFireSpit != null) enemyFireSpit.target = player;
    }

    void CleanupDeadEnemies()
    {
        // This loop checks for and removes defeated (destroyed) enemies from our tracking list.
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
            {
                aliveEnemies.RemoveAt(i);
                Debug.Log("An enemy was defeated. The next one can now be spawned.");
            }
        }
    }
}