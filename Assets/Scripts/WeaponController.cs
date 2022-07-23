using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponStats weaponStats;
    private Camera cam;
    private float nextFire;
    [SerializeField] private Transform tipOfGun;

    private void Start()
    {
        nextFire = weaponStats.fireRate;
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
                    nextFire = Time.time + weaponStats.fireRate;
                    ShotgunShot();
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
        GameObject bulletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation);
        var rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(CalculateDirection() * weaponStats.bulletSpeed);
    }

    private void ShotgunShot() //por corregir
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = Random.Range(-40f, 40f);
        euler.y = Random.Range(-40f, 40f);
        transform.eulerAngles = euler;

        GameObject pelletInstance = Instantiate(weaponStats.bullet, tipOfGun.position, tipOfGun.rotation);
        var rb = pelletInstance.GetComponent<Rigidbody>();
        rb.AddForce(euler * weaponStats.bulletSpeed);
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
