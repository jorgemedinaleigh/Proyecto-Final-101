using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject weaponInRadius;
    private WeaponController currentHeldWeapon;
    private Camera cam;

    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private GameObject weaponHolder;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        PickUpGun();
        DropGun();

        if (currentHeldWeapon != null && currentHeldWeapon.weaponStats.isRapid && Input.GetMouseButton(0))
        {
            currentHeldWeapon.Shoot();
        }
        if (currentHeldWeapon != null && !currentHeldWeapon.weaponStats.isRapid && Input.GetMouseButtonDown(0))
        {
            currentHeldWeapon.Shoot();
        }

        if(currentHeldWeapon != null && currentHeldWeapon.bulletsLeft < currentHeldWeapon.weaponStats.magazineSize && Input.GetKeyDown(reloadKey))
        {
            currentHeldWeapon.Reload();            
        }
    }

    private void PickUpGun()
    {
        if (Input.GetKeyDown(pickUpKey) && weaponInRadius != null && currentHeldWeapon == null)
        {
            weaponInRadius.GetComponent<Rigidbody>().isKinematic = true;
            weaponInRadius.GetComponent<Collider>().enabled = false;
            weaponInRadius.transform.SetParent(weaponHolder.transform);
            LeanTween.moveLocal(weaponInRadius, Vector3.zero, 0.3f);
            LeanTween.rotateLocal(weaponInRadius, Vector3.zero, 0.3f);
            currentHeldWeapon = weaponInRadius.GetComponent<WeaponController>();
        }        
    }
    private void DropGun()
    {
        if (Input.GetKeyDown(dropKey) && currentHeldWeapon != null)
        {
            weaponInRadius.transform.SetParent(null);
            currentHeldWeapon = null;
            var rb = weaponInRadius.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            weaponInRadius.GetComponent<Collider>().enabled = true;
            rb.AddForce(cam.transform.forward * 500);
            rb.angularVelocity = cam.transform.forward * 500;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("PickableGun") && weaponInRadius == null)
        {
            weaponInRadius = other.gameObject;
        }            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickableGun") && weaponInRadius != null)
        {
            weaponInRadius = null;
        }
    }
}
public enum WeaponType
{
    REVOLVER,
    SMG,
    SHOTGUN,
    RIFLE
}

