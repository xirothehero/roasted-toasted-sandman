using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cCutscene : MonoBehaviour
{
    public Sprite[] cutscenes;
    [SerializeField] private Image image;
    [SerializeField] private Text plotText;
    [SerializeField] private string[] lines;
    [SerializeField] private string nextScene;
    [SerializeField] private Image coverPanel;

    private bool isTransitioning;

    private int sceneIndex, lineIndex;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = 0;
        lineIndex = 0;
        image.sprite = cutscenes[sceneIndex];
        plotText.text = lines[lineIndex];

        StartCoroutine(FadeFromBlack());
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            lineIndex++;
            plotText.text = lines[(Mathf.Min(lineIndex, lines.Length-1))];

            if (lineIndex >= lines.Length)
            {
                sceneIndex++;

                if (sceneIndex >= cutscenes.Length)
                {
                    StartCoroutine(NextScene());
                }
                else
                {
                    StartCoroutine(Transition());
                }
            }
        }
    }

    private IEnumerator FadeToBlack()
    {
        isTransitioning = true;
        float fadeTime = 1f;
        float time = 0f;
        while (time <= fadeTime)
        {
            var tempColor = coverPanel.color;
            tempColor.a = Mathf.Lerp( 0, 1, time );
            coverPanel.color = tempColor;
            time += Time.deltaTime;
            yield return null;
        }

        isTransitioning = false;
    }

    private IEnumerator FadeFromBlack()
    {
        isTransitioning = true;
        float fadeTime = 1f;
        float time = 0f;
        while (time <= fadeTime)
        {
            var tempColor = coverPanel.color;
            tempColor.a = Mathf.Lerp( 1, 0, time );
            coverPanel.color = tempColor;
            time += Time.deltaTime;
            yield return null;
        }
        isTransitioning = false;
    }

    private IEnumerator Transition()
    {
        StartCoroutine(FadeToBlack());
        while (isTransitioning)
            yield return null;
        image.sprite = cutscenes[sceneIndex];
        StartCoroutine(FadeFromBlack());
        while (isTransitioning)
            yield return null;
        Debug.Log("Done");
    }

    private IEnumerator NextScene()
    {
        StartCoroutine(FadeToBlack());
        while (isTransitioning)
            yield return null;
        SceneManager.LoadScene(nextScene);
    }
}
