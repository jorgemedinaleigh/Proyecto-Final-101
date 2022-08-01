using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] float reaction;

    private NavMeshAgent agent;
    private GameObject player;
    private float currentEnemyHP;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        SetEnemyStats();
    }

    void Update()
    {
        Vector3 playerPosition = (player.transform.position - transform.position);
        PersuePlayer(playerPosition);
    }

    public void PersuePlayer(Vector3 playerPosition)
    {
        agent.destination = playerPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            currentEnemyHP = currentEnemyHP - collision.gameObject.GetComponent<BulletController>().bulletDamage;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - reaction);

            if(currentEnemyHP <= 0)
            {
                Destroy(gameObject, 0.5f);
            }
        }
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
    
    void SetEnemyStats()
    {
        currentEnemyHP = enemyStats.healthPoints;
        agent.speed = enemyStats.movementSpeed;
        agent.stoppingDistance = enemyStats.rangeToAttack;
    }
}

public enum EnemyType
{
    KAMIKAZE,
    MELEE,
    GUNNER,
    SNIPER
}
