using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraFollow : MonoBehaviour
{
    public Transform cameraFollowObj;
    public Transform cameraObj;
    public Transform playerObj;
    public float inputSensitivity = 5;
    public float yPosOffset = 3;

    public bool followPlayer = true;
    public float cinematicSpeed = 3f;
    public float cinematicTurnSpeed = 9f;
    // For the use of moving the camera somewhere in the scene to a destination


    public float turnAngleMinMax = 80;

    [Header("Cinematics")]
    public Transform destination;
    public Transform lookAtTarget;
    //public float maxDistance = 5f;
    //public float minDistance = 0.75f;
    private Vector3 offset;
    private Quaternion camRot;

    private RaycastHit hit;
    private Vector3 camera_offset;

    private void Start()
    {
        camRot = transform.localRotation;
        camera_offset = cameraObj.localPosition;
    }

    void Update()
    {
        if (followPlayer)
        {
            //transform.position = new Vector3(cameraFollowObj.position.x, cameraFollowObj.position.y + yPosOffset, cameraFollowObj.position.z);
            //transform.Rotate(0, Input.GetAxis("Mouse X") * inputSensitivity, 0);
            //if (transform.rotation.x > -turnAngleMinMax && transform.rotation.x < turnAngleMinMax)
            //{
            //    transform.Rotate(Input.GetAxis("Mouse Y") * inputSensitivity, 0, 0);
            //}

            camRot.x += Input.GetAxis("Mouse Y") * inputSensitivity * Time.deltaTime;
            camRot.y += Input.GetAxis("Mouse X") * inputSensitivity * Time.deltaTime;

            camRot.x = Mathf.Clamp(camRot.x, -turnAngleMinMax, turnAngleMinMax);

            transform.localRotation = Quaternion.Euler(camRot.x, camRot.y, camRot.z);

            //if ()
            //{
                //Debug.Log("Hit wall");
                //cameraObj.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, hit.point));
            //}
            if (!Physics.Linecast(transform.position, transform.position + transform.localRotation * camera_offset, out hit))
            {
                //Debug.Log("Jittering?");
                cameraObj.localPosition = Vector3.Lerp(cameraObj.localPosition, camera_offset, Time.deltaTime);
            }
            transform.position = playerObj.position;

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, cinematicSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, destination.rotation, cinematicTurnSpeed * Time.deltaTime);
            transform.LookAt(lookAtTarget.position);
        }

    }

    //private void FixedUpdate()
    //{
    //    if (followPlayer)
    //        transform.position = playerObj.position;
    //}

}