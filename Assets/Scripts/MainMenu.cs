using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(int level)
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + level);
    }

    public void Settings()
    {

    }

    public void QuitGame ()
    {
        Debug.Log("Goodbye World!");
        Application.Quit();
    }
}
