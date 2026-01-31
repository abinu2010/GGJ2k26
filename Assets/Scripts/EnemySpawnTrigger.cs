using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public Transform player;

    public GameObject smallEnemyPrefab;
    public int smallEnemyCount = 5;

    public GameObject bigEnemyPrefab;
    public int bigEnemyCount = 1;

    public Transform spawnPointsRoot;

    public bool spawnOnce = true;
    bool spawned;

    void OnTriggerEnter(Collider other)
    {
        if (spawnOnce && spawned) return;

        if (other.GetComponentInParent<PlayerHealth>() == null) return;

        SpawnWave();
        spawned = true;
    }

    void SpawnWave()
    {
        if (player == null) return;
        if (spawnPointsRoot == null) return;

        int spawnPointCount = spawnPointsRoot.childCount;
        if (spawnPointCount <= 0) return;

        for (int i = 0; i < smallEnemyCount; i++)
        {
            Transform spawnPoint = spawnPointsRoot.GetChild(i % spawnPointCount);
            SpawnOne(smallEnemyPrefab, spawnPoint);
        }

        for (int j = 0; j < bigEnemyCount; j++)
        {
            Transform spawnPoint = spawnPointsRoot.GetChild((smallEnemyCount + j) % spawnPointCount);
            SpawnOne(bigEnemyPrefab, spawnPoint);
        }
    }

    void SpawnOne(GameObject prefab, Transform spawnPoint)
    {
        if (prefab == null) return;

        GameObject enemyObj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        EnemyMovement enemyMovement = enemyObj.GetComponent<EnemyMovement>();
        if (enemyMovement != null) enemyMovement.target = player;

        EnemyAttack enemyAttack = enemyObj.GetComponent<EnemyAttack>();
        if (enemyAttack != null) enemyAttack.target = player;

        EnemyFireSpit enemyFireSpit = enemyObj.GetComponent<EnemyFireSpit>();
        if (enemyFireSpit != null) enemyFireSpit.target = player;
    }
}
