using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTunnelController : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float waitToSpawn = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemyWave(int waveNumber)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            Instantiate(enemies[i % 4], transform.position, transform.rotation);
            SpawnManager.enemiesCount++;

            yield return new WaitForSeconds(waitToSpawn);
        }
    }

    public void SpawnNextWave(int waveNumber)
    {
        StartCoroutine(SpawnEnemyWave(waveNumber));
    }
}
