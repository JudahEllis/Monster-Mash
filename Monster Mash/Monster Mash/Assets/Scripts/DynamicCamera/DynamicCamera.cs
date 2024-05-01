using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynamicCamera : MonoBehaviour
{
    public List<Transform> playerTransforms;

    Bounds characterBounds;

    //Cinemachine Virtual Camera Used for the Stage
    //The Cam is set to Follow the GameObject this script is on
    [SerializeField]
    CinemachineVirtualCamera dynamicCam;

    //Animtion Curve that Determines the FoV of the Camera
    [SerializeField]
    AnimationCurve camFovCurve;

    //The Min and Max Position of the cameras Follow Target
    [SerializeField]
    Transform[] clampValues;

    //The Min and Max Transform on the stage. only calculated on the X-Axis
    //Both Objects should have the same values except for their X
    [SerializeField]
    Transform[] stageClampValues;

    //The Max Distance of the Stage itself
    //Used to normalize the Bound distance
    [SerializeField]
    float maxDist;

    void Start()
    {
        dynamicCam.m_Lens.FieldOfView = 40f;

        maxDist = Vector3.Distance(stageClampValues[0].transform.position, stageClampValues[1].transform.position);
    }

    private void LateUpdate()
    {
        DynamicCameraFunction();
    }

    void DynamicCameraFunction()
    {
        characterBounds = new Bounds(playerTransforms[0].position, Vector3.zero);

        foreach (Transform player in playerTransforms)
        {
            characterBounds.Encapsulate(player.position);
        }

        Vector2 value = new Vector2(Mathf.Abs(characterBounds.center.normalized.x), Mathf.Abs(characterBounds.center.normalized.y)).normalized;

        float clampX = Mathf.Clamp(characterBounds.center.x, clampValues[0].transform.position.x, clampValues[1].transform.position.x);

        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);

        float dist = Vector3.Distance(characterBounds.min, characterBounds.max);

        float normalizedDist = dist / maxDist;

        dynamicCam.m_Lens.FieldOfView = camFovCurve.Evaluate(normalizedDist);
    }
}
