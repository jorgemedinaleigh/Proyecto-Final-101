using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] PickUpStats pickUpStats;
    [SerializeField] float degreesPerSecond;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.GetComponentInChildren<WeaponController>() == null)
            {
                return;
            }

            switch (pickUpStats.pickUpType)
            {
                case PickUpType.ARMOR:
                    other.GetComponentInParent<PlayerStatsController>().playerArmor += pickUpStats.pointsToAdd;
                    break;
                case PickUpType.MEDKIT:
                    other.GetComponentInParent<PlayerStatsController>().playerHP += pickUpStats.pointsToAdd;
                    break;
                default:
                    other.GetComponentInChildren<WeaponController>().totalBullets += pickUpStats.pointsToAdd;
                    break;
            }

            Destroy(gameObject);
        }
    }
}

public enum PickUpType
{
    ARMOR,
    MEDKIT,
    REVOLVERAMMO,
    SHOTGUNAMMO,
    SMGAMMO
}
