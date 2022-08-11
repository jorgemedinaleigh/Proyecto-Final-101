using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] Animator animController;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float blastRadius;
    [SerializeField] float blastForce;

    private NavMeshAgent agent;
    private GameObject player;
    private float currentEnemyHP;
    bool isAttacking;

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
                StartCoroutine(WaitForHitAnimation());               
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

    IEnumerator WaitForAttackAnimation()
    {
        if (animController != null)
        {
            animController.SetTrigger("Attack");

            while (!animController.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {   //Wait until the enemy is in the Attack state
                yield return null;
            }

            yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    IEnumerator WaitForHitAnimation()
    {
        if(animController != null)
        {
            animController.SetTrigger("Hit");

            while(!animController.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                yield return null;
            }

            yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public IEnumerator AttackPlayer()
    {        
        isAttacking = true; // atack started

        animController.SetFloat("Speed", 0);
        agent.speed = 0;
        yield return StartCoroutine(WaitForAttackAnimation());

        switch (enemyStats.enemyType)
        {
            case EnemyType.MELEE:
                break;
            case EnemyType.KAMIKAZE:
                Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
                foreach(Collider nearbyObject in colliders)
                {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                    }                                        
                }
                Destroy(gameObject, 0.1f);
                SpawnManager.enemiesCount--;
                GameObject hitInstance = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                yield break;
            case EnemyType.SNIPER:
                break;
            case EnemyType.GUNNER:
                break;
        }

        animController.SetFloat("Speed", enemyStats.movementSpeed);
        agent.speed = enemyStats.movementSpeed;

        yield return null; // esperar un fotograma
        isAttacking = false;
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
