using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    public Camera editorCamera;
    public GameObject limbPlacer;
    public bool cameraRotating = false;
    void Update()
    {
        if (cameraRotating == false)
        {
            transform.position = Input.mousePosition;

            Ray ray = editorCamera.ScreenPointToRay(this.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                limbPlacer.transform.position = Vector3.Lerp(limbPlacer.transform.position, hit.point, 0.15f);
                limbPlacer.transform.LookAt(-hit.normal);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }


    }
}
