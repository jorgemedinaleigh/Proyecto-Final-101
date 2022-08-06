using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject spawnManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject weapon;
    [SerializeField] TextMeshProUGUI waveNumberText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI playerArmorText;
    [SerializeField] TextMeshProUGUI weaponAmmoText;

    float timer;
    float playerHP;
    float playerArmor;
    float weaponAmmo;
    float ammoLeft;
    float enemyWave;

    void Start()
    {
        timer = 0;
        timerText.text = timer.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        DisplayTime(timer);
        DisplayEnemyWave();
        DisplayPlayerStats();
        DisplayWeaponAmmo();
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void DisplayEnemyWave()
    {
        enemyWave = spawnManager.GetComponent<SpawnManager>().waveNumber;
        waveNumberText.text = "Wave: " + enemyWave.ToString();
    }

    void DisplayPlayerStats()
    {
        playerHP = player.GetComponent<PlayerController>().playerHP;
        playerArmor = player.GetComponent<PlayerController>().playerArmor;
        playerHPText.text = playerHP.ToString();
        playerArmorText.text = playerArmor.ToString();
    }

    void DisplayWeaponAmmo()
    {
        if(weapon.GetComponentInChildren<WeaponController>() != null)
        {
            weaponAmmo = weapon.GetComponent<WeaponController>().totalBullets;
            ammoLeft = weapon.GetComponent<WeaponController>().bulletsLeft;
            weaponAmmoText.text = ammoLeft.ToString() + "/" + weaponAmmo.ToString();
        }
        else
        {
            weaponAmmoText.text = "NO WEAPON";
        }
    }
}
