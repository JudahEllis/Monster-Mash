using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterRot : MonoBehaviour
{
    private Transform mainCameraTransform;
    [SerializeField] private CharacterController cont;

    // Fixed rotations for facing left and right (in degrees)
    private float fixedRotationLeft = -30f;//37f;
    private float fixedRotationRight = 30f;//-218f;

    private bool faceRight = true;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float dotProduct = Vector3.Dot(cont.transform.forward.normalized, Vector3.right.normalized);

        if (dotProduct > 0.5f)
        {
            faceRight = true;
        }
        else if (dotProduct < -0.5f)
        {
            faceRight = false;
        }

        Quaternion targetRot = Quaternion.identity;

        if (faceRight)
        {
            targetRot = Quaternion.Euler(0, fixedRotationRight, 0);
            //print("right");
        }
        else
        {
            targetRot = Quaternion.Euler(0, fixedRotationLeft, 0);
            //print("left");
        }

        transform.localRotation = targetRot;

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);

        /*// Calculate the look direction towards the camera
        Vector3 lookDirection = mainCameraTransform.position - transform.position;
        lookDirection.y = 0f; // Keep the character's orientation strictly 2D

        if (lookDirection != Vector3.zero)
        {
            // Calculate the rotation to look at the camera
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            // Calculate the angle between the character's forward and the camera's forward
            float angle = Vector3.SignedAngle(transform.forward, mainCameraTransform.forward, Vector3.up);

            // Snap the rotation to the fixed angles based on the angle between the character and the camera
            if (angle >= -45f && angle < 45f)
            {
                // Facing right
                targetRotation = Quaternion.Euler(0f, fixedRotationRight, 0f);
            }
            else
            {
                // Facing left
                targetRotation = Quaternion.Euler(0f, fixedRotationLeft, 0f);
            }

            // Smoothly rotate the character towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        // Your character movement logic (using the Character Controller) goes here
        // ...*/
    }
}
