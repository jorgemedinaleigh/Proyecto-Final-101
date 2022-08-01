using UnityEngine;

public class LavaPitController : MonoBehaviour
{
    [SerializeField] float movementRange;
    [SerializeField] float movementSpeed;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        Vector3 panelPosition = startingPosition;
        panelPosition.z = panelPosition.z + movementRange * Mathf.Sin(movementSpeed * Time.time);
        transform.position = panelPosition;
    }
}
