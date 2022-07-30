using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int waveNumber = 1;
    [SerializeField] float waitToSpawn = 2f;

    private int bossCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    void Update()
    {
        SpawnNextWave();
    }

    IEnumerator SpawnEnemyWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            Instantiate(enemies[i % 4], transform.position, transform.rotation);
            yield return new WaitForSeconds(waitToSpawn);
        }
    }

    void SpawnNextWave()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            waveNumber++;
            StartCoroutine(SpawnEnemyWave());
        }
    }
}
