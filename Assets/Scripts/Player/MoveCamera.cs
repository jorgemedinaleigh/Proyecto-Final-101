using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform player;

    void Update()
    {
        if (GameUtils.IsGamePaused){ return; }
        
        transform.position = player.transform.position;
    }
}
