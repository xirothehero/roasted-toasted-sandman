using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Button start, exit;

    private AudioSource click;
    void Awake()
    {
        start.onClick.AddListener(Begin);
        exit.onClick.AddListener(Exit);
        click = GetComponent<AudioSource>();
    }

    private void Begin()
    {
        // Can be changed to change into cutscenes and stuff
        click.Play();
        SceneManager.LoadScene("IntroScene");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
