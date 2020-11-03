using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] WaveConfig bossWaveConfig;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    [SerializeField] int spawnCountToReset = 0;
    //[SerializeField] int scoreToNextStage = 50;

    //cahced variable
    int waveCounter = 0;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
            //if you'd rather use score before getting to the boss...
            //(FindObjectOfType<GameSession>().GetScore() > scoreToNextStage
            if (waveCounter > spawnCountToReset)
            {
                looping = false;
            }
        }
        while (looping);

        SpawnBoss(bossWaveConfig);
    }
    private void Update()
    {
        
    }

    private IEnumerator SpawnAllWaves()
  
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
                var currentWave = waveConfigs[waveIndex];
            waveCounter++;
            yield return StartCoroutine(spawnAllEnemiesInWave(currentWave));
        }
    }
    private IEnumerator spawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    public void SpawnBoss(WaveConfig waveConfig)
    { 
        var newEnemy = Instantiate(
            waveConfig.GetEnemyPrefab(),
            waveConfig.GetWaypoints()[0].transform.position,
            Quaternion.identity);

            newEnemy.GetComponent<BossPathing>().SetBossWaveConfig(waveConfig);
    }
}

