using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resume, exit;
    // Start is called before the first frame update
    void Awake()
    {
        resume.onClick.AddListener(Resume);
        exit.onClick.AddListener(Exit);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void Exit()
    {
        Time.timeScale = 1;
        // Put in when Main Menu scene is made
        // SceneManager.LoadScene("MainMenu");
    }
}
