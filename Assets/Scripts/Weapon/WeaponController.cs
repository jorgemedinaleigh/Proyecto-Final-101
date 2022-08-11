using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponStats weaponStats;
    private Camera cam;
    private float nextFire;
    public int bulletsLeft;
    private int bulletsShot;
    private bool reloading = false;
    private AudioSource shotSFX;
    private AudioSource reloadSFX;

    [SerializeField] private Transform tipOfGun;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] public float totalBullets;

    private void Awake()
    {
        nextFire = weaponStats.fireRate;
        bulletsLeft = weaponStats.magazineSize;
    }

    private void Start()
    {
        cam = Camera.main;
        shotSFX = GetComponent<AudioSource>();        
    }

    public void Shoot()
    {                
        if (reloading)
        {
             return; 
        }
        else if(Time.time >= nextFire && bulletsLeft > 0)
        {
            nextFire = Time.time + weaponStats.fireRate;
            shotSFX.Play();

            if(weaponStats.weaponType == WeaponType.SHOTGUN)
            {
                bulletsShot = weaponStats.bulletsPerTap;
                ShotgunShot();
                bulletsLeft--;
            }
            else
            {
                OneShot();
            }
        }
        else if(bulletsLeft <= 0)
        {
            Reload();
        }        
    }

    private void OneShot()
    {
        GameObject bulletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation.normalized * Quaternion.Euler(90f, 0f, 0f));
        var rb = bulletInstance.GetComponent<Rigidbody>();
        bulletInstance.GetComponent<BulletController>().bulletDamage = weaponStats.damagePerShot;
        rb.AddForce(CalculateDirection() * weaponStats.bulletSpeed);
        GetComponent<Animator>().SetTrigger("Shoot");
        GameObject hitInstance = Instantiate(muzzleFlash, tipOfGun.position, tipOfGun.rotation);
        Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

        bulletsLeft--;
    }

    private void ShotgunShot()
    {
        float x = Random.Range(-weaponStats.spread, weaponStats.spread);
        float y = Random.Range(-weaponStats.spread, weaponStats.spread);

        GameObject bulletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation);
        var rb = bulletInstance.GetComponent<Rigidbody>();
        bulletInstance.GetComponent<BulletController>().bulletDamage = weaponStats.damagePerShot;
        rb.AddForce(CalculateDirection() * weaponStats.bulletSpeed + new Vector3(x, y, 0f));
        GetComponent<Animator>().SetTrigger("Shoot");
        GameObject hitInstance = Instantiate(muzzleFlash, tipOfGun.position, tipOfGun.rotation);
        Destroy(hitInstance, hitInstance.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

        bulletsShot--;

        if(bulletsShot > 0)
        {
            Invoke(nameof(ShotgunShot), 0f);
        }
    }

    private Vector3 CalculateDirection()
    {
        var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 3f));
        RaycastHit hit;
        Vector3 dir;
        if (Physics.Raycast(ray, out hit))
        {
            dir = (hit.point - tipOfGun.position).normalized;
        }            
        else
        {
            dir = ray.direction;
        }
            
        return dir;
    }

    public void Reload()
    {
        if(!reloading)
        {
            reloading = true;            
            GetComponent<Animator>().SetTrigger("Reload");
            reloadSFX = gameObject.transform.parent.GetComponent<AudioSource>();
            reloadSFX.Play();
            Invoke("ReloadFinished", weaponStats.reloadTime);
        }
    }

    private void ReloadFinished()
    {
        bulletsLeft = weaponStats.magazineSize;
        reloading = false;
    }
}
