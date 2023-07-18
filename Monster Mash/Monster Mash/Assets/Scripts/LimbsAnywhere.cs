using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbsAnywhere : MonoBehaviour
{
    Camera mainCamera;

    private GameObject limbPref;

    //private SelectionManager manager;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        //manager = FindObjectOfType<SelectionManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast and check if it hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider has a reference to the GameObject you want to detect clicks on
                if (hit.collider.CompareTag("Limbable"))
                {
                    // The player clicked on the collider of the desired GameObject
                    //Vector3 clickPosition = hit.point;
                    //Debug.Log("Clicked on: " + hit.collider.gameObject.name + " at position: " + hit.point);

                    // Calculate the position and rotation for the new limb
                    Vector3 clickPosition = hit.point;

                    // Calculate the direction from the limb to the torso
                    Vector3 directionToTorso = hit.transform.position - clickPosition;

                    // Determine the rotation to face the torso
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTorso, Vector3.up);

                    CreateLimb(hit, targetRotation);
                }
            }
        }
    }

    void CreateLimb(RaycastHit hit, Quaternion rot)
    {
        if (SelectionManager.Instance.GetSelectedPrefab())
        {
            //print("new limb!");

            limbPref = SelectionManager.Instance.GetSelectedPrefab();

            GameObject newLimb = Instantiate(limbPref, hit.point, Quaternion.identity);

            Transform closestBone = FindClosestBone(hit);

            newLimb.transform.rotation = rot;

            //trying this new thing hahahehehoo
            GameObject emptyParent = new GameObject();
            emptyParent.transform.parent = closestBone;
            emptyParent.transform.position = newLimb.transform.position;
            emptyParent.transform.rotation = Quaternion.identity;

            //newLimb.transform.parent = closestBone;
            newLimb.transform.parent = emptyParent.transform;

            SelectionManager.Instance.SetSelectedPrefab(null);
        }
    }

    private Transform FindClosestBone(RaycastHit hit)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = hit.collider.GetComponent<SkinnedMeshRenderer>();
        Transform closestBone = null;
        float closestDistance = Mathf.Infinity;

        if (skinnedMeshRenderer != null)
        {
            Transform[] bones = skinnedMeshRenderer.bones;

            foreach (Transform bone in bones)
            {
                float distance = Vector3.Distance(bone.position, hit.point);
                if (distance < closestDistance)
                {
                    closestBone = bone;
                    closestDistance = distance;
                }
            }
        }

        return closestBone;
    }
}
