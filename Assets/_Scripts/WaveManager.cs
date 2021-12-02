using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveConfig{
    public float timeSpawn;
    public int[] numberEnemy;
}

public class WaveManager : MonoBehaviour {
    public static WaveManager instance;

    [SerializeField] [Min(1)] int enemyAddToWave = 2;
    [Header ("Waves Configuration")]
    [SerializeField]
    private  WaveConfig[] waves;
    [Header ("House of Enemies Configuration")]
    [SerializeField]
    private  Transform[] spawnPositions;
    [Header ("Enemies Configuration")]
    [SerializeField]
    private  GameObject[] enemies;
    [Header ("Other Configurations")]
    [SerializeField]
    private  TMP_Text currentWaveNumber;

    [SerializeField]
    private Animator currentWaveAnimator;
    [SerializeField]
    private  AudioSource nextWaveSoundEffect;

    private int currentEnemiesCount;
    private float currentTimeToSpawn;
    private int waveEnemies;
    private int waveIndex;
    private Coroutine waveCoroutine;

    private static readonly int Play = Animator.StringToHash("Play");
    private static readonly int Lose = Animator.StringToHash("Lose");

    void Awake()
    {
        instance = this;
        currentEnemiesCount = waves[waves.Length - 1].numberEnemy.Length + enemyAddToWave;
        currentTimeToSpawn = waves[waves.Length - 1].timeSpawn;
    }

    private void Start()
    {
        EventsManager.instance.onGameStart += Instance_onGameStart;
    }

    private void OnDestroy()
    {
        EventsManager.instance.onGameStart -= Instance_onGameStart;
    }

    private void Instance_onGameStart(bool state)
    {
        if (state)
        {
            StartSpawnWaves();
        }
        else
        {
            PlayerDead();
        }
    }

    void StartSpawnWaves()
    {
        waveCoroutine = StartCoroutine(nameof(WaveController));
        currentWaveAnimator.SetBool(Lose, false);
        waveIndex = 0;
    }

    private IEnumerator WaveController()
    {
        while (true)
        {
            nextWaveSoundEffect.Play();
            currentWaveNumber.text = $"Wave {waveIndex + 1}";
            currentWaveAnimator.SetTrigger(Play);
            UIManager.instance.AddWave();

            if (waveIndex < waves.Length)
            {
                foreach (var enemy in waves[waveIndex].numberEnemy)
                {
                    yield return new WaitForSeconds(waves[waveIndex].timeSpawn);
                    SpawnEnemy(enemy);
                    waveEnemies++;
                }
            }
            else
            {
                for (int i = 0; i < currentEnemiesCount; i++)
                {
                    yield return new WaitForSeconds(currentTimeToSpawn);
                    SpawnEnemy();
                    waveEnemies++;
                }
                currentEnemiesCount += enemyAddToWave;
            }

            waveIndex++;

            yield return new WaitUntil(() => waveEnemies == 0);
        }
    }

    public void EnemyDead()
    {
        waveEnemies--;
    }

    void PlayerDead()
    {
        currentWaveNumber.text = $"YOU LOSE";
        currentWaveAnimator.SetBool(Lose, true);
        currentWaveAnimator.SetTrigger(Play);
        StopCoroutine(waveCoroutine);
    }

    private void SpawnEnemy(int numberEnemy){
        float spawnPoint = Random.Range(0, spawnPositions.Length);
        var go = Instantiate(enemies[numberEnemy], spawnPositions[(int)spawnPoint].position, Quaternion.identity);
        //go.transform.Rotate(0, spawnPoint == 0 ? 180 : 0, 0);
    }

    private void SpawnEnemy()
    {
        float spawnPoint = Random.Range(0, spawnPositions.Length);
        var go = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPositions[(int)spawnPoint].position, Quaternion.identity);
        //go.transform.Rotate(0, spawnPoint == 0 ? 180 : 0, 0);
    }
}