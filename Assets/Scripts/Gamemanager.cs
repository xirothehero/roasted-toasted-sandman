using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public sPlayer thePlayer;
    public GameObject gameOverCanvas;
    public sCameraFollow mainCamera;

    public cButton_NPC_Interactions npcButtonManager;

    private void Awake()
    {
        Time.timeScale = 1;
        if (instance == null || instance != this)
        {
            //DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
