using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int waveNumber = 1;
    [SerializeField] float waitToSpawn = 2f;

    private int bossCount = 0;
    private IEnumerator coroutine;

    void Start()
    {
        SpawnEnemyWave();
    }

    void Update()
    {
        SpawnNextWave();
    }  

    IEnumerator SpawnEnemy(int index)
    {        
        Instantiate(enemies[index], transform.position, enemies[index].transform.rotation);
        yield return new WaitForSeconds(waitToSpawn);
    }

    void SpawnEnemyWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            coroutine = SpawnEnemy(i);
            StartCoroutine(coroutine);
            StopCoroutine(coroutine);
        }
    }

    void SpawnNextWave()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            waveNumber++;
            SpawnEnemyWave();
        }
    }
}
