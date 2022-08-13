using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] Animator animController;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Transform attackPoint;

    NavMeshAgent agent;
    GameObject player;
    PlayerStatsController playerStatsController;
    float currentEnemyHP;
    bool isAttacking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerStatsController = player.GetComponent<PlayerStatsController>();

        SetEnemyStats();

        if (animController != null)
        {
            animController.SetBool("IsAlive", true);
        }
    }

    void Update()
    {
        if(player == null)
        {   // early return
            return;
        }

        Vector3 playerPosition = player.transform.position;
        
        if(!isAttacking)
        {            
            PersuePlayer(playerPosition);
            if(enemyStats.rangeToAttack >= Vector3.Distance(transform.position, playerPosition))
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    public void PersuePlayer(Vector3 playerPosition)
    {
        transform.LookAt(playerPosition);
        agent.destination = playerPosition;

        if(animController != null)
        {
            animController.SetFloat("Speed", agent.speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            currentEnemyHP = currentEnemyHP - collision.gameObject.GetComponent<BulletController>().bulletDamage;

            if (animController != null)
            {
                animController.SetTrigger("Hit");
            }
            if(currentEnemyHP < 0)
            {
                currentEnemyHP = 0;
            }
            if(currentEnemyHP == 0)
            {
                agent.speed = 0;
                GetComponent<BoxCollider>().enabled = false;
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
            {   //Wait until the enemy is in the Dead state
                yield return null;
            }
            
            yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
        }

        Destroy(gameObject, 0.5f);
        SpawnManager.enemiesCount--;
    }

    public IEnumerator AttackPlayer()
    {        
        isAttacking = true; // atack started

        animController.SetFloat("Speed", 0);
        agent.speed = 0;
        animController.SetTrigger("Attack");

        switch (enemyStats.enemyType)
        {
            case EnemyType.MELEE:
                if (enemyStats.rangeToAttack >= Vector3.Distance(attackPoint.position, player.transform.position))
                {
                    playerStatsController.HandleDamage(enemyStats.damagePerAttack);
                }
                break;
            case EnemyType.KAMIKAZE:
                yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
                if (enemyStats.rangeToAttack >= Vector3.Distance(attackPoint.position, player.transform.position))
                {
                    playerStatsController.HandleDamage(enemyStats.damagePerAttack);
                }
                Destroy(gameObject, 0.1f);
                SpawnManager.enemiesCount--;
                GameObject hitInstance = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                yield break;
            case EnemyType.SNIPER:
                ShootPlayer();
                break;
            case EnemyType.GUNNER:
                ShootPlayer();
                break;
        }

        yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;        
        animController.SetFloat("Speed", enemyStats.movementSpeed);
        agent.speed = enemyStats.movementSpeed;
    }  
    
    void ShootPlayer()
    {
        GameObject bulletInstance = Instantiate(enemyStats.bullet, attackPoint.position, attackPoint.rotation);
        var rb = bulletInstance.GetComponent<Rigidbody>();
        bulletInstance.GetComponent<EnemyBulletController>().bulletDamage = enemyStats.damagePerAttack;
        rb.AddRelativeForce(Vector3.forward * enemyStats.bulletSpeed, ForceMode.Impulse);
    }

    void SetEnemyStats()
    {
        currentEnemyHP = enemyStats.healthPoints;
        agent.speed = enemyStats.movementSpeed;
        agent.stoppingDistance = enemyStats.rangeToAttack;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, enemyStats.rangeToAttack);
    }
}

public enum EnemyType
{
    KAMIKAZE,
    MELEE,
    GUNNER,
    SNIPER
}
