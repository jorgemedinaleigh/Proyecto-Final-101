using UnityEngine;

[CreateAssetMenu(menuName = "PickUp Manager/PickUp Stats")]
public class PickUpStats : ScriptableObject
{
    public int pointsToAdd;
    public PickUpType pickUpType;
}
