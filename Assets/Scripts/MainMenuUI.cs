using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance.menuMusic);
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
}
