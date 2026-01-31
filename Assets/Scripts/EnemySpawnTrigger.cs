using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public Transform player;
    public Transform spawnPointsRoot;

    public bool spawnOnce = true;

    public WaveStep[] steps;

    bool started;
    bool finished;
    int stepIndex;
    float nextStepTime;

    readonly List<GameObject> aliveEnemies = new List<GameObject>();


    void OnTriggerEnter(Collider other)
    {
        if (finished) return;
        if (spawnOnce && started) return;

        if (other.GetComponentInParent<Health>() == null) return;
        Debug.Log("Trigger hit");


        started = true;
        stepIndex = 0;
        nextStepTime = Time.time;
        RunStep();
    }

    void Update()
    {
        if (!started) return;
        if (finished) return;

        CleanupDead();

        if (steps == null || steps.Length == 0)
        {
            finished = true;
            return;
        }

        if (stepIndex >= steps.Length)
        {
            finished = true;
            gameObject.SetActive(false);
            return;
        }

        WaveStep step = steps[stepIndex];

        if (step.waitUntilAllDead && aliveEnemies.Count > 0) return;
        if (Time.time < nextStepTime) return;

        RunStep();
    }

    void RunStep()
    {
        if (player == null) return;
        if (spawnPointsRoot == null) return;

        int spawnPointCount = spawnPointsRoot.childCount;
        if (spawnPointCount <= 0) return;

        if (stepIndex >= steps.Length) return;

        WaveStep step = steps[stepIndex];

        if (step.clearAliveListBeforeSpawn)
        {
            aliveEnemies.Clear();
        }

        if (step.spawns != null)
        {
            for (int i = 0; i < step.spawns.Length; i++)
            {
                SpawnEntry entry = step.spawns[i];
                if (entry.prefab == null) continue;
                if (entry.count <= 0) continue;

                for (int c = 0; c < entry.count; c++)
                {
                    int spawnIndex = (entry.spawnPointStartIndex + c) % spawnPointCount;
                    Transform spawnPoint = spawnPointsRoot.GetChild(spawnIndex);

                    float posX = spawnPoint.position.x;
                    float posY = spawnPoint.position.y;
                    float posZ = spawnPoint.position.z;

                    GameObject enemyObj = Instantiate(entry.prefab, new Vector3(posX, posY, posZ), spawnPoint.rotation);
                    aliveEnemies.Add(enemyObj);

                    EnemyMovement enemyMovement = enemyObj.GetComponent<EnemyMovement>();
                    if (enemyMovement != null) enemyMovement.target = player;

                    EnemyAttack enemyAttack = enemyObj.GetComponent<EnemyAttack>();
                    if (enemyAttack != null) enemyAttack.target = player;

                    EnemyFireSpit enemyFireSpit = enemyObj.GetComponent<EnemyFireSpit>();
                    if (enemyFireSpit != null) enemyFireSpit.target = player;
                }
            }
        }

        stepIndex++;
        nextStepTime = Time.time + step.delayAfterSpawn;
    }

    void CleanupDead()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
            {
                aliveEnemies.RemoveAt(i);
            }
        }
    }
}

[System.Serializable]
public class WaveStep
{
    public bool waitUntilAllDead = true;
    public float delayAfterSpawn = 0.5f;
    public bool clearAliveListBeforeSpawn = false;
    public SpawnEntry[] spawns;
}

[System.Serializable]
public class SpawnEntry
{
    public GameObject prefab;
    public int count = 1;
    public int spawnPointStartIndex = 0;
}
