using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] float reaction;
    [SerializeField] private Animator animController;

    private NavMeshAgent agent;
    private GameObject player;
    private float currentEnemyHP;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        SetEnemyStats();
        
        animController.SetBool("IsAlive", true);
    }

    void Update()
    {
        if (player == null)
        {   // early return
            return;
        }

        Vector3 playerPosition = (player.transform.position - transform.position);
        PersuePlayer(playerPosition);
    }

    public void PersuePlayer(Vector3 playerPosition)
    {
        agent.destination = playerPosition;
        animController.SetFloat("Speed", agent.speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            currentEnemyHP = currentEnemyHP - collision.gameObject.GetComponent<BulletController>().bulletDamage;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - reaction);

            animController.SetTrigger("Hit");
            
            if(currentEnemyHP <= 0)
            {
                animController.SetBool("IsAlive", false);
            }

            StartCoroutine(WaitForDeadAnimation());
        }
    }

    IEnumerator WaitForDeadAnimation()
    {
        while (!animController.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {   // esperar hasta que este en este estado
            yield return null;
        }

        yield return new WaitForSeconds(animController.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject, 0.5f);
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
