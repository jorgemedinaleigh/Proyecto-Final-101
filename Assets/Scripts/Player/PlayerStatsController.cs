using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] public float playerArmor;
    [SerializeField] public float playerHP;

    public bool isDead = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lava") && !isDead)
        {
            isDead = true;
        }
    }
}
