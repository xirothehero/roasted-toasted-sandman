using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public float health = 100;
    public float speed = 8;
    public Transform theCamera;
    float heading = 0;
    float keep = 0;
    void Update()
    {
        float zInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");


        Rotate(xInput, zInput);

        if (zInput != 0 || xInput != 0)
        {
            if (zInput == 1 || zInput == -1)
                keep = zInput;
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    void Rotate(float _xInput, float _zInput)
    {
        // Forward direction: camPos -> playerPos\\
        Vector3 cameraVector = new Vector3(transform.position.x - Camera.main.transform.position.x, 0.0f,
                                                   transform.position.z - Camera.main.transform.position.z);

        // Calculate the look direction of the player based on the input and the cameraVector
        Vector3 playerLookDirection = Quaternion.LookRotation(cameraVector) * new Vector3(_xInput, 0.0f, _zInput);

        if (playerLookDirection != Vector3.zero)
        {
            Quaternion destRot = Quaternion.LookRotation(playerLookDirection);
            transform.rotation = destRot;
        }
        else
        {
            if (keep == 1)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
            }
            else
            {
                Vector3 lookPos = theCamera.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;
            }
        }
    }
}
