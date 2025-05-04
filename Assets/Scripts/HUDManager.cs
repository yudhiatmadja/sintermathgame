using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public GameObject pausePanel; 
    public GameObject levelPanel;
    private bool isPaused = false; 
    private bool isLevelOpen;

    private void Start()
    {
        pausePanel.SetActive(false);
        levelPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelPanel.activeSelf) 
            {
                levelPanel.SetActive(false); 
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; 
        pausePanel.SetActive(false);

        if (isLevelOpen)
        {
            levelPanel.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void LevelPanel()
    {
        pausePanel.SetActive(false);
        levelPanel.SetActive(true);
        isLevelOpen = true;
    }

    public void CloseLevelPanel()
    {
        levelPanel.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        if (SceneManager.GetActiveScene().name == levelName)
        {
            CloseLevelPanel(); 
        }
        else
        {
            Time.timeScale = 1; 
            SceneManager.LoadScene(levelName);
        }
    }
}
