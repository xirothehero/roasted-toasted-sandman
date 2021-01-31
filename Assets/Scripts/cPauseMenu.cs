using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resume, exit;

    [SerializeField] private AudioSource click;

    // Start is called before the first frame update
    void Awake()
    {
        resume.onClick.AddListener(Resume);
        exit.onClick.AddListener(Exit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Resume()
    {
        click.Play();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void Exit()
    {
        click.Play();
        Time.timeScale = 1;
        // Put in when Main Menu scene is made
        SceneManager.LoadScene("MainMenu");
    }
}
