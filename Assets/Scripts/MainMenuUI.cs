using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance.menuMusic);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                PauseMenu();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Main"); // your game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // so you see it in editor
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // important if coming from pause/death
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main");

    }

    public void PauseMenu()
    {
        {
            isPaused = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

}
