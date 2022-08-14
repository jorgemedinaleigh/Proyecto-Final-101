using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject weaponInRadius;
    private static WeaponController currentHeldWeapon;
    private Camera cam;

    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private GameObject weaponHolder;

    public static WeaponController CurrentWeapon 
    { 
        get { return currentHeldWeapon; }
    }

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
        if (Input.GetKeyDown(pickUpKey) && weaponInRadius != null)
        {
            if(currentHeldWeapon != null)
            {
                Destroy(weaponHolder.transform.GetChild(0).gameObject);
            }
            weaponInRadius.GetComponent<Rigidbody>().isKinematic = true;
            weaponInRadius.GetComponent<Collider>().enabled = false;
            weaponInRadius.transform.SetParent(weaponHolder.transform);
            LeanTween.moveLocal(weaponInRadius, Vector3.zero, 0.3f);
            LeanTween.rotateLocal(weaponInRadius, Vector3.zero, 0.3f);
            currentHeldWeapon = weaponInRadius.GetComponent<WeaponController>();
            weaponInRadius = null;
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

    private void OnDestroy()
    {
        currentHeldWeapon = null;
    }
}
public enum WeaponType
{
    REVOLVER,
    SMG,
    SHOTGUN,
    RIFLE
}

