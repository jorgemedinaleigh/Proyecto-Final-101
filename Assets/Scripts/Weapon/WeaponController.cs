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
        
        switch (weaponStats.weaponType)
        {
            case WeaponType.REVOLVER:
                if (Time.time >= nextFire)
                {                                        
                    if(bulletsLeft > 0)
                    {
                        nextFire = Time.time + weaponStats.fireRate;
                        OneShot();
                        shotSFX.Play();
                    }
                    else
                    {
                        Reload();
                    }
                }
                break;
            case WeaponType.SMG:
                if (Time.time >= nextFire)
                {
                    if (bulletsLeft > 0)
                    {
                        nextFire = Time.time + weaponStats.fireRate;
                        OneShot();
                        shotSFX.Play();
                    }
                    else
                    {
                        Reload();
                    }
                }
                break;
            case WeaponType.SHOTGUN:
                if(Time.time >= nextFire)
                {
                    if(bulletsLeft > 0)
                    {
                        bulletsShot = weaponStats.bulletsPerTap;
                        nextFire = Time.time + weaponStats.fireRate;
                        ShotgunShot();
                        bulletsLeft--;
                        shotSFX.Play();
                    }
                    else
                    {
                        Reload();
                    }
                }
                break;
            case WeaponType.RIFLE:
                if (Time.time >= nextFire)
                {
                    if (bulletsLeft > 0)
                    {
                        nextFire = Time.time + weaponStats.fireRate;
                        OneShot();
                    }
                    else
                    {
                        Reload();
                    }
                }
                break;
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
            reloadSFX = gameObject.transform.parent.GetComponent<AudioSource>();
            GetComponent<Animator>().SetTrigger("Reload");
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
