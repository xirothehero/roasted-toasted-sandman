using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sItempickup : MonoBehaviour
{
    public string itemName = "";

    [Header("Speed")]
    public float spinSpeed = 0.3f;
    public float floatingSpeed = 0.5f;

    //public int sandGain = 5;

    [Header("Parameters")]
    public float height = 2.5f;

    private float orgPosY;
    private float actualHeight;
    bool up = true;

    private void Start()
    {
        orgPosY = transform.position.y;
        actualHeight = transform.position.y + height;
    }
    private void LateUpdate()
    {
        transform.Rotate(0,spinSpeed*Time.deltaTime,0, Space.World);
        if (up)
        {
            transform.Translate(Vector3.up * floatingSpeed * Time.deltaTime, Space.Self);
            if (transform.position.y >= actualHeight)
                up = false;
        }
        else
        {
            transform.Translate(Vector3.down * floatingSpeed * Time.deltaTime, Space.Self);
            if (transform.position.y <= orgPosY)
                up = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<sPlayer>().sand+=sandGain;
            //other.gameObject.GetComponent<sPlayer>().sandText.text = "Sand: " + other.gameObject.GetComponent<sPlayer>().sand;
            other.gameObject.GetComponent<sPlayer>().itemsCollected.Add(itemName);
            Destroy(gameObject);
        }
    }
}
