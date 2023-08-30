using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowObj;     // Reference to the object the camera should follow

    [Range(0f, 1f)]
    public float Stickiness = 0.8f; // A value between 0 and 1 controlling the smoothness of camera movement

    private Vector3 offset;         // The initial offset between the camera and the object it follows
    const float epsilon = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial offset between the camera and the follow object
        offset = transform.position - FollowObj.position;
    }

    private void FixedUpdate()
    {
        // Smoothly move the camera towards the target position using Lerp
        transform.position = Vector3.Lerp(transform.position, FollowObj.position + offset, Stickiness);
    }
}
