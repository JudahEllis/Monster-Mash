using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterRot : MonoBehaviour
{
    private Transform mainCameraTransform;
    [SerializeField] private CharacterController cont;

    // Fixed rotations for facing left and right (in degrees)
    private float fixedRotationLeft = -30f;
    private float fixedRotationRight = 30f;

    private bool faceRight = true;

    private Quaternion targetRot;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;

        cont = FindObjectOfType<CharacterController>();
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
    }
}
