﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cGameover : MonoBehaviour
{
    public void GotoScene(int _idx)
    {
        SceneManager.LoadScene(_idx);
    }
}
