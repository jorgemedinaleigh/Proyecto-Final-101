using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    public float bulletDamage = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "PickableGun" && collision.gameObject.tag != "Bullet")
        {
            GameObject hitInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
            Destroy(gameObject, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        }        
    }
}
