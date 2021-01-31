using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cFollowCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Gamemanager.instance.mainCamera.transform.GetChild(0).localPosition);
    }
}
