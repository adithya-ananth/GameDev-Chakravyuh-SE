using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CameraFollowSideScrollVariant : MonoBehaviour
{
    public float FollowSpeed = 1f;  // Speed at which camera follows
    public Transform target;        // The character (or object) the camera will follow
    public float dampingFactor = 0.2f; // Damping factor to control smoothness of follow

    private Vector3 velocity = Vector3.zero;  // Smooth damping velocity

    // Update is called once per frame
    void Update()
    {
        // Desired position, no offset, and z fixed (camera follows the player both in X and Y)
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Smoothly move the camera to the target position using the damping factor
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampingFactor / FollowSpeed);
    }
}