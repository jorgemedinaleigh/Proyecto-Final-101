using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float bulletDamage = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStatsController>().HandleDamage(bulletDamage);
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            return;
        }
        Destroy(gameObject);
    }
}
