using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponStats weaponStats;
    private Camera cam;
    private float nextFire;
    private int bulletsLeft;
    private int bulletsShot;
    private bool shooting;
    private bool reloading;

    [SerializeField] private Transform tipOfGun;

    private void Awake()
    {
        nextFire = weaponStats.fireRate;
        bulletsLeft = weaponStats.magazineSize;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void Shoot()
    {
        switch (weaponStats.weaponType)
        {
            case WeaponType.REVOLVER:
                if (Time.time >= nextFire)
                {
                    nextFire = Time.time + weaponStats.fireRate;
                    OneShot();
                }
                break;
            case WeaponType.SMG:
                if (Time.time >= nextFire)
                {
                    nextFire = Time.time + weaponStats.fireRate;
                    OneShot();
                }
                break;
            case WeaponType.SHOTGUN:
                if(Time.time >= nextFire)
                {
                    bulletsShot = weaponStats.bulletsPerTap;
                    nextFire = Time.time + weaponStats.fireRate;
                    ShotgunShot();
                    bulletsLeft--;
                }
                break;
            case WeaponType.RIFLE:
                if (Time.time >= nextFire)
                {
                    nextFire = Time.time + weaponStats.fireRate;
                    OneShot();
                }
                break;
        }
    }

    private void OneShot()
    {
        float x = Random.Range(-weaponStats.spread, weaponStats.spread);
        float y = Random.Range(-weaponStats.spread, weaponStats.spread);

        GameObject bulletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation);
        var rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(CalculateDirection() * weaponStats.bulletSpeed + new Vector3(x, y, 0));

        bulletsLeft--;
    }

    private void ShotgunShot()
    {
        float x = Random.Range(-weaponStats.spread, weaponStats.spread);
        float y = Random.Range(-weaponStats.spread, weaponStats.spread);

        GameObject bulletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation);
        var rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(CalculateDirection() * weaponStats.bulletSpeed + new Vector3(x, y, 0));

        bulletsShot--;

        if(bulletsShot > 0)
        {
            Invoke("ShotgunShot", 0);
        }
    }

    private Vector3 CalculateDirection()
    {
        var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 3f));
        RaycastHit hit;
        Vector3 dir;
        if (Physics.Raycast(ray, out hit))
            dir = (hit.point - tipOfGun.position).normalized;
        else
            dir = ray.direction;
        return dir;
    }
}
