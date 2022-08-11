using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject deathMenuUI;
    [SerializeField] PlayerStatsController player;
    [SerializeField] TextMeshProUGUI finalWaveText;
    [SerializeField] TextMeshProUGUI timeSurvivedText;

    float finalWave;
    float timeSurvived;

    void Update()
    {
        CheckEscapeKey();
        PlayerDeath();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CheckEscapeKey()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void PlayerDeath()
    {
        if(player.isDead)
        {
            Time.timeScale = 0f;
            deathMenuUI.SetActive(true);
            gameUI.SetActive(false);
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            var uiController = gameObject.GetComponentInChildren<UIController>(true);
            
            finalWave = uiController.enemyWave - 1f;
            finalWaveText.text = "Waves Overcome: " + finalWave.ToString();

            timeSurvived = uiController.timer;
            float minutes = Mathf.FloorToInt(timeSurvived / 60);
            float seconds = Mathf.FloorToInt(timeSurvived % 60);
            float milliseconds = (timeSurvived % 1) * 1000;
            timeSurvivedText.text = "Time Survived: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }
}
