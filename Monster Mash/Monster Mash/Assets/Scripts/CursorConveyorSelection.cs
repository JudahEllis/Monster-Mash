using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorConveyorSelection : MonoBehaviour
{
    private CustomCursor myCursor;

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private GameObject currentSelection;
    private bool hasSelection = false;

    // Start is called before the first frame update
    void Start()
    {
        myCursor = GetComponent<CustomCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSelection)
        {
            Vector3 cursorPositionWorld = cam.ScreenToWorldPoint(myCursor.GetCursorPos());
            currentSelection.transform.position = cursorPositionWorld;

            Ray ray = cam.ScreenPointToRay(myCursor.GetCursorPos());

            // Calculate the intersection point with the plane (assuming the plane is parallel to the camera's "down" direction)
            Vector3 intersectionPoint = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);

            // Set the position of the GameObject to the intersection point
            currentSelection.transform.position = intersectionPoint;

            if (!myCursor.GetButtonHeld())
            {
                StopSelection();
            }
        }
    }

    private void NewSelection()
    {
        currentSelection.GetComponent<Rigidbody>().isKinematic = true;
        currentSelection.GetComponent<OnConveyor>().enabled = false;
        currentSelection.GetComponent<Collider>().enabled = false;
    }

    private void StopSelection()
    {
        currentSelection.GetComponent<Rigidbody>().isKinematic = false;
        currentSelection.GetComponent<OnConveyor>().enabled = true;
        currentSelection.GetComponent<OnConveyor>().NotOnBelt();
        currentSelection.GetComponent<Collider>().enabled = true;
        currentSelection = null;
        hasSelection = false;
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

    private bool Click()
    {
        Vector3 imageCenterWorldPos = myCursor.GetCursorPos();

        // Cast a ray from the center of the UI Image toward the world
        Ray ray = cam.ScreenPointToRay(imageCenterWorldPos);
        RaycastHit hitInfo;

        // Perform the raycast
        if (Physics.Raycast(ray, out hitInfo, mask) && hitInfo.collider.GetComponent<OnConveyor>())
        {
            // Do something with the hitInfo, e.g., get the hit object's name
            hitInfo.collider.GetComponent<OnConveyor>().ClickedOn(this);
            Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);
            return true;
        } else
        {
            return false;
        }
    }
}
