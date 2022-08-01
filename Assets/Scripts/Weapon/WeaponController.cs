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
        switch (weaponStats.weaponType)
        {
            case WeaponType.REVOLVER:
                if (Time.time >= nextFire)
                {
                    if(bulletsLeft > 0 && !reloading)
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
                    if (bulletsLeft > 0 && !reloading)
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
                    if(bulletsLeft > 0 && !reloading)
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
                    if (bulletsLeft > 0 && !reloading)
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

        bulletsShot--;

        if(bulletsShot > 0)
        {
            Invoke("ShotgunShot", 0f);
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
        reloading = true;
        reloadSFX = gameObject.transform.parent.GetComponent<AudioSource>();
        StartCoroutine(ReloadAnimation(weaponStats.reloadTime));
        reloadSFX.Play();
        Invoke("ReloadFinished", weaponStats.reloadTime);        
    }

    private void ReloadFinished()
    {
        bulletsLeft = weaponStats.magazineSize;
        reloading = false;
    }

    IEnumerator ReloadAnimation(float duration)
    {
        float startAngle = transform.eulerAngles.x;
        float endAngle = startAngle + 360f;
        float time = 0f;

        Quaternion initialRotation = transform.rotation;

        while(time < duration)
        {
            time = time + Time.deltaTime;
            float xRotation = Mathf.Lerp(startAngle, endAngle, time / duration) % 360;
            transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);

            yield return null;
        }

        transform.rotation = initialRotation;
    }
}
