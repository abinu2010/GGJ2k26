using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;

    public GameObject[] enemyPrefabs;

    public int totalToSpawn = 8;
    public float timeBetweenSpawns = 0.7f;
    public int maxAliveAtOnce = 1;

    public float minPlayerDistanceToSpawn = 5f;
    public float spawnOffsetX = 0.35f;

    bool started;
    bool finished;

    int spawnedCount;
    float nextSpawnTime;

    readonly List<GameObject> aliveEnemies = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (finished) return;
        if (started) return;

        Health health = other.GetComponentInParent<Health>();
        if (health == null) return;

        if (player == null) player = health.transform;

        started = true;
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        if (!started) return;
        if (finished) return;

        CleanupDead();

        if (spawnedCount >= totalToSpawn)
        {
            if (aliveEnemies.Count == 0)
            {
                finished = true;
                gameObject.SetActive(false);
            }
            return;
        }

        if (Time.time < nextSpawnTime) return;

        if (maxAliveAtOnce > 0 && aliveEnemies.Count >= maxAliveAtOnce)
        {
            nextSpawnTime = Time.time + 0.1f;
            return;
        }

        if (player == null) return;
        if (spawnPoint == null) return;
        if (enemyPrefabs == null) return;
        if (enemyPrefabs.Length == 0) return;

        float playerPosX = player.position.x;
        float spawnPosX = spawnPoint.position.x;

        float absDistance = Mathf.Abs(playerPosX - spawnPosX);
        if (absDistance < minPlayerDistanceToSpawn)
        {
            nextSpawnTime = Time.time + 0.1f;
            return;
        }

        SpawnOne();

        spawnedCount++;
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

    void SpawnOne()
    {
        int prefabIndex = spawnedCount % enemyPrefabs.Length;
        GameObject prefab = enemyPrefabs[prefabIndex];
        if (prefab == null) return;

        float posX = spawnPoint.position.x;
        float posY = spawnPoint.position.y;
        float posZ = spawnPoint.position.z;

        float offsetX = Random.Range(-spawnOffsetX, spawnOffsetX);

        GameObject enemyObj = Instantiate(prefab, new Vector3(posX + offsetX, posY, posZ), spawnPoint.rotation);
        aliveEnemies.Add(enemyObj);

        EnemyMovement enemyMovement = enemyObj.GetComponent<EnemyMovement>();
        if (enemyMovement != null) enemyMovement.target = player;

        EnemyAttack enemyAttack = enemyObj.GetComponent<EnemyAttack>();
        if (enemyAttack != null) enemyAttack.target = player;

        EnemyFireSpit enemyFireSpit = enemyObj.GetComponent<EnemyFireSpit>();
        if (enemyFireSpit != null) enemyFireSpit.target = player;
    }

    void CleanupDead()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null) aliveEnemies.RemoveAt(i);
        }
    }
}
