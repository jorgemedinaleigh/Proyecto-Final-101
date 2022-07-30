using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Manager/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float healthPoints;
    public float rangeToAttack;
    public float damagePerAttack;
    public float movementSpeed;
    public float bulletSpeed;
    public GameObject bullet;
    public EnemyType enemyType;
}
