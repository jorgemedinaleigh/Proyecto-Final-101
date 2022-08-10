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

    public float timer;
    public float enemyWave;
    float playerHP;
    float playerArmor;
    float weaponAmmo;
    float ammoLeft;
    
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

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    void DisplayEnemyWave()
    {
        enemyWave = spawnManager.GetComponent<SpawnManager>().waveNumber;
        waveNumberText.text = "Wave: " + enemyWave.ToString();
    }

    void DisplayPlayerStats()
    {
        playerHP = player.GetComponent<PlayerStatsController>().playerHP;
        playerArmor = player.GetComponent<PlayerStatsController>().playerArmor;
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
