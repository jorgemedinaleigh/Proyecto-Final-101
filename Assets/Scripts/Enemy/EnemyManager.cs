using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyController enemyController;

    void Start()
    {
        enemyController = gameObject.GetComponent<EnemyController>();
    }

    void Update()
    {
        
    }
}

public enum EnemyType
{
    KAMIKAZE,
    MELEE,
    GUNNER,
    SNIPER
}
