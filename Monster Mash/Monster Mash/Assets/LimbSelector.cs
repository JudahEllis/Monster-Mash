using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR;

public class LimbSelector : MonoBehaviour
{
    public struct State
    {
        public enum States
        {
            noSelection,
            limbSelected,
            onTorso
        }

        States currState;
        public void ChangeState(States state)
        {
            currState = state;
        }

        public States GetState() { return currState; }
    }

    State limbState;

    public Camera editorCamera;
    public Camera mainCamera;

    [SerializeField] GameObject cursor_control;

    [HideInInspector]
    public bool cameraRotating = false;

    [HideInInspector]
    public GameObject limbToPlace = null;

    // Start is called before the first frame update
    void Awake()
    {
        limbState.ChangeState(State.States.noSelection);
    }

    // Update is called once per frame
    void Update()
    {
        switch(limbState.GetState())
        {
            case State.States.noSelection:
                NoLimbSelected();
                break;
            case State.States.limbSelected:
                LimbSelected();
                break;
            case State.States.onTorso:
                break;
            default:
                break;
        }
    }


    void NoLimbSelected()
    {
        // scenario when a limb has yet to be selected
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast and check if it hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider has a reference to a Limb
                if (hit.collider.CompareTag("Limb"))
                {
                    //Raycast collided with the monster part's "Cube" collider, therefore take
                    //the cube's parent (i.e. the monster part) and make it the selected prefab
                    SelectionManager.Instance.SetSelectedPrefab(hit.collider.transform.parent.gameObject);

                    //Assign the prefab to the local Limb
                    limbToPlace = SelectionManager.Instance.GetSelectedPrefab();

                    // the collider is necessary for raycasting, but once the part has been selected,
                    // it should no longer have a collider as that may cause problems
                    hit.collider.enabled = false;

                    //change what the MainCamera is looking at
                    //mainCamera.cullingMask = LayerMask.GetMask("UI");
                    //editorCamera.cullingMask = LayerMask.GetMask("Default", "Torso", "Parts");

                    //StartCoroutine(MoveLimbToCenter());

                    //cursor_control.GetComponent<LimbOnTorso>().limbSelected = true;

                    limbState.ChangeState(State.States.limbSelected);

                    Debug.Log("limbAssigned: " + limbToPlace.name);
                }
            }
        }
    }

    private void LimbSelected()
    {

        limbToPlace.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60f));

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    // Perform the raycast and check if it hits a collider
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        // Check if the collider has a reference to the GameObject you want to detect clicks on
        //        if (hit.collider.CompareTag("Limbable"))
        //        {
        //            //// Calculate the position and rotation for the new limb
        //            //Vector3 clickPosition = hit.point;

        //            //// Calculate the direction from the limb to the torso
        //            //Vector3 directionToTorso = hit.transform.position - clickPosition;

        //            //// Determine the rotation to face the torso
        //            //Quaternion targetRotation = Quaternion.LookRotation(directionToTorso, Vector3.up);

        //            // Calculate the direction from the limb to the torso
        //            Vector3 directionToTorso = hit.transform.position - hit.point;

        //            // Determine the rotation to face the torso
        //            Quaternion targetRotation = Quaternion.LookRotation(directionToTorso, -hit.normal);

        //            targetRotation = Quaternion.RotateTowards(targetRotation, Quaternion.identity, 50);

        //            CreateLimb(hit, targetRotation);

        //            Destroy(cursor_control.GetComponent<cursor_limbplacer>().limbToPlace);
        //        }
        //    }
        //
    }
}
