using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    Vector2 offset;

    float speed = 0.025f;

    [SerializeField] private Vector3 direction;

    float objSpeed = 0.125f;

    private GameObject currentSelection;
    private bool hasSelection = false;

    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset.y += -speed * Time.deltaTime;
        offset.y %= 1f;
        /*if (offset.y >= 0.25f)
        {
            offset.y = 0.0f;
        }*/

        GetComponent<Renderer>().material.mainTextureOffset = offset;

        if (hasSelection)
        {
            //Vector3 mousePositionWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, currentSelection.transform.position.y, Input.mousePosition.y));
            //currentSelection.transform.position = mousePositionWorld;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // Calculate the intersection point with the plane (assuming the plane is parallel to the camera's "down" direction)
            Vector3 intersectionPoint = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);

            // Set the position of the GameObject to the intersection point
            currentSelection.transform.position = intersectionPoint;

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopSelection();
            }
        }
    }

    private void NewSelection()
    {
        currentSelection.GetComponent<Rigidbody>().isKinematic = true;
        currentSelection.GetComponent<OnConveyor>().enabled = false;
    }

    private void StopSelection()
    {
        currentSelection.GetComponent<Rigidbody>().isKinematic = false;
        currentSelection.GetComponent<OnConveyor>().enabled = true;
        currentSelection = null;
        hasSelection = false;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public float GetSpeed()
    {
        return objSpeed;
    }

    public void SetSelection(GameObject part)
    {
        if (!hasSelection)
        {
            hasSelection = true;
            currentSelection = part;
            NewSelection();
        }
    }
}
