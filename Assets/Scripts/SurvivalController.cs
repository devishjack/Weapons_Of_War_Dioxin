using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalController : MonoBehaviour
{
    public GameObject enemy;

    public static SurvivalController instance;

    public SpawnPoint[] spawnPoints;

    private bool waveEnd;
    private int waveCount;
    private int enemyCount;
    private float fade;
    public int enemiesLeft;
    private bool firstRound;

    public TMPro.TMP_Text waveText;

    private void Awake()
    {
        instance = this;
        waveCount = 1;
        waveEnd = true;
        enemyCount = 5;
        waveText.alpha = 0;
        fade = 5;
        firstRound = true;
    }

    private void Update()
    {
        if(enemiesLeft == 0)
        {
            waveEnd = true;
        }

        if (waveEnd)
        {
            WaveIncrease();
        }
        else
        {
            fade -= Time.deltaTime;
            waveText.alpha = fade;
        }
    }

    private void WaveStart()
    {
        waveText.alpha = 256;
        FindObjectOfType<AudioManager>().Play("RoundStart");
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemy, GetSpawnPoint());
        }
        enemiesLeft = enemyCount;
        waveEnd = false;
        firstRound = false;
    }

    private void WaveIncrease()
    {
        if (!firstRound)
        {
            waveCount++;
            enemyCount += 5;
        }
        waveText.text = "Wave " + waveCount.ToString();
        WaveStart();
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
