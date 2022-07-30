using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats;

    private NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 playerPosition = (player.transform.position - transform.position).normalized;
        PersuePlayer(playerPosition);
    }

    public void PersuePlayer(Vector3 playerPosition)
    {        
        transform.LookAt(playerPosition);
        agent.destination = playerPosition;
    }

    public void AttackPlayer()
    {
        switch(enemyStats.enemyType)
        {
            case EnemyType.MELEE:
                break;
            case EnemyType.KAMIKAZE:
                break;
            case EnemyType.SNIPER:
                break;
            case EnemyType.GUNNER:
                break;
        }        
    }    
}
