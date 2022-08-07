using UnityEngine;

[CreateAssetMenu(menuName = "PickUp Manager/PickUp Stats")]
public class PickUpStats : ScriptableObject
{
    public float pointsToAdd;
    public PickUpType pickUpType;
}
