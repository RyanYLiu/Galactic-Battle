﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    [SerializeField] int numLoops = 3;
    bool stopSpawning = false;
    [SerializeField] float spawnTimer = 60f;
    [SerializeField] float maxRandomSpawnTime = 60f;
    [SerializeField] float minRandomSpawnTime = 30f;
    [SerializeField] GameObject warningCanvas;
    int numWavesSpawned = 0;
    [SerializeField] int minWavesBeforeBoss = 5;
    [SerializeField] float chanceToSpawnBoss = 0f;
    [SerializeField] float bossChanceIncrement = 0.05f;
    Pauser pauser;
    bool spawning = false;

    // Start is called before the first frame update
    // IEnumerator Start()
    // {
    //     do
    //     {
    //         yield return StartCoroutine(SpawnAllWaves());
    //     }
    //     while (looping);
    // }

    void Start()
    {
        pauser = FindObjectOfType<Pauser>();
    }

    void Update()
    {
        if (pauser.IsPaused()) { return; }
        
        if (stopSpawning)
        {
            if (FindObjectsOfType<Enemy>().Length == 0)
            {
                warningCanvas.gameObject.SetActive(true);
                GetComponent<EnemySpawner>().enabled = false;
            }
        }
        else
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && !spawning)
            {
                spawning = true;
                SpawnWave(Random.Range(0, waveConfigs.Count - 1));
                numWavesSpawned += 1;
                if (numWavesSpawned > minWavesBeforeBoss)
                {
                    chanceToSpawnBoss += bossChanceIncrement;
                    if (Random.Range(0f, 1f) < chanceToSpawnBoss)
                    {
                        stopSpawning = true;
                    }
                }
            }
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minRandomSpawnTime, maxRandomSpawnTime);
        spawning = false;
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private void SpawnWave(int waveIndex)
    {
        StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);    // so we don't have to add config manually per enemy
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
            while (pauser.IsPaused())
            {
                yield return null;
            }
        }
        ResetSpawnTimer();
    }
}
