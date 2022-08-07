using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] public int waveNumber = 1;
    [SerializeField] GameObject[] spawnTunnels;

    public static int enemiesCount = 0;

    void Start()
    {
        waveNumber = 1;    
    }

    void Update()
    {
        SpawnWave();
    }

    void SpawnWave()
    {
        if(enemiesCount == 0)
        {
            Debug.Log(waveNumber);
            for (int i = 0; i < spawnTunnels.Length; i++)
            {
                var spawnTunnel = spawnTunnels[i].GetComponent<SpawnTunnelController>();
                spawnTunnel.SpawnNextWave(waveNumber);
            }
            waveNumber++;
        }
        else if(enemiesCount < 0)
        {
            CheckEnemyCount();
        }        
    }

    void CheckEnemyCount()
    {
        if(FindObjectsOfType<EnemyController>().Length >= 0)
        {
            enemiesCount = FindObjectsOfType<EnemyController>().Length;
        }
    }
}
