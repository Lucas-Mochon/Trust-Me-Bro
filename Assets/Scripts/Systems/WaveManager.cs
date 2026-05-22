using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WaveData
{
    public GameObject[] enemyTypes;
    public int enemyCount = 4;
    public float spawnInterval = 1f;
    public Transform[] spawnPoints;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveData[] waves;

    Action onComplete;
    int activeEnemies;

    public void StartWaves(Action onAllDone = null)
    {
        onComplete = onAllDone;
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        foreach (WaveData wave in waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitUntil(() => activeEnemies <= 0);
        }
        onComplete?.Invoke();
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        if (wave.enemyTypes == null || wave.enemyTypes.Length == 0) yield break;
        if (wave.spawnPoints == null || wave.spawnPoints.Length == 0) yield break;

        activeEnemies = 0;
        for (int i = 0; i < wave.enemyCount; i++)
        {
            GameObject prefab = wave.enemyTypes[i % wave.enemyTypes.Length];
            Transform pt      = wave.spawnPoints[i % wave.spawnPoints.Length];
            if (prefab == null || pt == null) continue;

            GameObject enemy = Instantiate(prefab, pt.position, Quaternion.identity);
            activeEnemies++;
            StartCoroutine(TrackEnemy(enemy));

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    IEnumerator TrackEnemy(GameObject enemy)
    {
        yield return new WaitUntil(() => enemy == null);
        activeEnemies--;
    }
}
