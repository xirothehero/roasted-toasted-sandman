using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBillboardSprite : MonoBehaviour
{
    [SerializeField] private Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (theCamera)
            transform.rotation = theCamera.transform.rotation;
        else
            transform.rotation = Gamemanager.instance.mainCamera.transform.rotation;
    }
}
