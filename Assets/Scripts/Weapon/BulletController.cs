using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletDamage = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "PickableGun" && collision.gameObject.tag != "Bullet")
        {            
            Destroy(gameObject);
        }        
    }
}
