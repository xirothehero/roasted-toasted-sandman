using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject brokenObj;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            gameObject.SetActive(false);
            Instantiate(brokenObj, transform.position, transform.rotation);
            print("FRACTURE!");
        }
    }

}
