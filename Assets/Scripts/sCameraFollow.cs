using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraFollow : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 8;
    public float yPosOffset = 3;

    private Vector3 offset;
    
    private void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + yPosOffset, player.position.z - Vector3.Distance(transform.position, player.position));
    }
    void Update()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X")*turnSpeed, Vector3.up) * offset;
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
    }
}
