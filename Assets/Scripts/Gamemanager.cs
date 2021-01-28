using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public sPlayer thePlayer;
    public GameObject gameOverCanvas;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            //DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
