using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cCutscene : MonoBehaviour
{
    public Sprite[] cutscenes;
    [SerializeField] private Image image;
    [SerializeField] private string nextScene;

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        image.sprite = cutscenes[currentIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex++;

            if (currentIndex >= cutscenes.Length)
            {
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                image.sprite = cutscenes[currentIndex];
            }
        }
    }
}
