using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }


    void Update()
    {
        PursuePlayer();
    }

    void PursuePlayer()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        transform.position = transform.position + lookDirection * speed * Time.deltaTime;
    }
}
