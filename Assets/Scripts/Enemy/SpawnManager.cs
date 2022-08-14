using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] public int waveNumber;
    [SerializeField] GameObject[] spawnTunnels;

    public static int enemiesCount = 0;    

    void Update()
    {
        SpawnWave();
    }

    void SpawnWave()
    {
        CheckEnemyCount();
        if (enemiesCount == 0)
        {
            waveNumber++;
            Debug.Log(waveNumber);
            for (int i = 0; i < spawnTunnels.Length; i++)
            {
                var spawnTunnel = spawnTunnels[i].GetComponent<SpawnTunnelController>();
                spawnTunnel.SpawnNextWave(waveNumber);
            }            
        }
    }

    void CheckEnemyCount()
    {
        enemiesCount = FindObjectsOfType<EnemyController>().Length;
    }

    void OnDestroy()
    {
        enemiesCount = 0;
    }
}
