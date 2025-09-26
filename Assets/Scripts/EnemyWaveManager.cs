using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fastEnemyPrefab;
    [SerializeField]
    private GameObject normalEnemyPrefab;
    [SerializeField]
    private GameObject BigEnemyPrefab;
    [SerializeField]
    private EnemySpawnPoint[] enemySpawnPoints;

    [SerializeField]
    private AnimationCurve waves;

    private float timeIncrement = 0f;

    // Update is called once per frame
    void Update()
    {
        // splits a 2 min period into 10 segments, and sends a wave every segment
        //if (Time.timeSinceLevelLoad / 60f > timeIncrement)
        //{
        //    Debug.Log("SENDING WAVE");
        //    timeIncrement += 0.1f;
        //    StartCoroutine(SendWave(Mathf.RoundToInt(waves.Evaluate(timeIncrement) * 2f)));
        //    Debug.Log(Time.timeSinceLevelLoad + " : " + waves.Evaluate(timeIncrement) + " : sending wave with " + Mathf.RoundToInt(waves.Evaluate(timeIncrement) * 10f) + " enemies");
        //}
    }

    public void StartWaves()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            timeIncrement += 0.1f;
            StartCoroutine(SendWave(Mathf.RoundToInt(waves.Evaluate(timeIncrement) * 2f)));
            yield return new WaitForSeconds(Mathf.Clamp(timeIncrement*5f, 0f, 10f));
            if (timeIncrement >= 1f)
                timeIncrement = 0f;
        }
    }

    private IEnumerator SendWave(int enemyNum)
    {
        for (int i = 0; i < enemyNum; i++)
        {
            SpawnEnemy(PickRandomEnemy());
            yield return new WaitForSeconds(Random.Range(0, 5f));
        }
    }

    private GameObject PickRandomEnemy()
    {
        // first 20% of game only spawns normal enemies
        if (timeIncrement < 0.2f)
            return normalEnemyPrefab;

        float num = Random.Range(0f, 1f);
        if (num < 0.3f)
            return normalEnemyPrefab;
        else if (num < 0.8f)
            return fastEnemyPrefab;
        else
            return BigEnemyPrefab;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        int num = Random.Range(0, enemySpawnPoints.Length);
        EnemySpawnPoint spawnPoint = enemySpawnPoints[num];
        Enemy newEnemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.SetGoal(spawnPoint.GetGoal());
    }
}
