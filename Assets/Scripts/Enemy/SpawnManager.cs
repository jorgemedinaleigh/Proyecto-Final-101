using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int waveNumber = 1;
    [SerializeField] GameObject[] spawnTunnels;

    public static int enemiesCount = 0;

    void Start()
    {
        
    }

    void Update()
    {
        SpawnWave();
        Debug.Log(enemiesCount);
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
    }
}
