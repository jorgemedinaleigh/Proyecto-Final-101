using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] Animator animController;

    private NavMeshAgent agent;
    private GameObject player;
    private float currentEnemyHP;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        SetEnemyStats();

        if (animController != null)
        {
            animController.SetBool("IsAlive", true);
        }
    }

    void Update()
    {
        if (player == null)
        {   // early return
            return;
        }

        Vector3 playerPosition = player.transform.position;
        PersuePlayer(playerPosition);

        if(enemyStats.rangeToAttack >= Vector3.Distance(transform.position, playerPosition))
        {
            AttackPlayer();
        }
    }

    public void PersuePlayer(Vector3 playerPosition)
    {
        agent.destination = playerPosition;

        if (animController != null)
        {
            animController.SetFloat("Speed", agent.speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            currentEnemyHP = Mathf.Clamp(currentEnemyHP - collision.gameObject.GetComponent<BulletController>().bulletDamage, 0f, enemyStats.healthPoints);

            if (animController != null)
            {
                animController.SetTrigger("Hit");
            }
            if(currentEnemyHP == 0)
            {
                agent.speed = 0;
                StartCoroutine(SetAndWaitForDeathAnimation());
            }
        }
    }

    IEnumerator SetAndWaitForDeathAnimation()
    {
        if (animController != null)
        {
            animController.SetBool("IsAlive", false);
            
            while (!animController.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {   // esperar hasta que este en este estado
                yield return null;
            }
            
            yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
        }

        Destroy(gameObject, 0.5f);
        SpawnManager.enemiesCount--;
    }

    public void AttackPlayer()
    {
        animController.SetFloat("Speed", 0);
        agent.speed = 0;

        if (animController != null)
        {
            animController.SetTrigger("Attack");
        }

        switch (enemyStats.enemyType)
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

        agent.speed = enemyStats.movementSpeed;
        animController.SetFloat("Speed", agent.speed);
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
