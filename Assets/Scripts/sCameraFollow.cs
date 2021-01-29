using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraFollow : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 8;
    public float yPosOffset = 3;

    public bool followPlayer = true;
    public float cinematicSpeed = 3f;
    public float cinematicTurnSpeed = 9f;
    // For the use of moving the camera somewhere in the scene to a destination
    public Transform destination;
    public Transform lookAtTarget;
    private Vector3 offset;
    
    private void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + yPosOffset, player.position.z - Vector3.Distance(transform.position, player.position));
    }
    void Update()
    {
        if (followPlayer)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            if (player != null)
            {
                transform.position = player.position + offset;
                transform.LookAt(player.position);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, cinematicSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, destination.rotation, cinematicTurnSpeed * Time.deltaTime);
            transform.LookAt(lookAtTarget.position);
        }

    }
}
