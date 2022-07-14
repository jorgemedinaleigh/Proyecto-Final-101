using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeController : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        transform.LookAt(lookDirection);
        transform.position = transform.position + lookDirection * speed * Time.deltaTime;
    }
}
