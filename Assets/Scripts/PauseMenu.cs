using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject EndgameMenuObject;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale >= 0)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Endgame()
    {
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
    }
}
