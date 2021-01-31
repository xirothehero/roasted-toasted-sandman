using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Button start, exit;
    void Awake()
    {
        start.onClick.AddListener(Begin);
        exit.onClick.AddListener(Exit);
    }

    private void Begin()
    {
        // Can be changed to change into cutscenes and stuff
        SceneManager.LoadScene("Level");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
