using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] public float playerArmor;
    [SerializeField] public float playerHP;

    public bool isDead = false;

    float damageDiff;

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

    public void HandleDamage(float damage)
    {
        if(playerArmor <= 0)
        {
            playerHP -= damage;
            playerArmor = 0;
            HandleDeath();
        }
        else if(playerArmor - damage >= 0)
        {
            playerArmor -= damage;
        }
        else
        {
            damageDiff = Mathf.Abs(playerArmor - damage);
            playerArmor = 0;
            playerHP -= damageDiff;
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        if(playerHP <= 0)
        {
            isDead = true;
        }
    }    
}
