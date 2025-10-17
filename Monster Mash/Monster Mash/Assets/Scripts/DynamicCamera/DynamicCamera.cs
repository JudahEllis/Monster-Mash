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
    Transform[] clampValuesX;

    [SerializeField]
    Transform[] clampValuesY;

    [SerializeField]
    Transform[] clampValuesZ;

    //The Min and Max Transform on the stage. only calculated on the X-Axis
    //Both Objects should have the same values except for their X
    [SerializeField]
    Transform[] stageClampValues;

    //The Max Distance of the Stage itself
    //Used to normalize the Bound distance
    [SerializeField]
    float maxDist;

    [SerializeField]
    AnimationCurve zClampCurve;

    public static DynamicCamera Instance { get; private set; }

    void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        maxDist = Vector3.Distance(stageClampValues[0].transform.position, stageClampValues[1].transform.position);

        zClampCurve = new AnimationCurve(new Keyframe(0, clampValuesZ[1].position.z),
            new Keyframe(1, clampValuesZ[0].position.z));
    }

    private void LateUpdate()
    {
        
    }

    private void Update()
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

        float dist = Vector3.Distance(characterBounds.min, characterBounds.max);

        float normalizedDist = dist / maxDist;

        float clampX = Mathf.Clamp(characterBounds.center.x, clampValuesX[0].transform.position.x, clampValuesX[1].transform.position.x);

        float clampY = Mathf.Clamp(characterBounds.center.y, clampValuesY[0].transform.position.y, clampValuesY[1].transform.position.y);

        float clampZ = zClampCurve.Evaluate(normalizedDist);

        //transform.position = new Vector3(clampX, clampY, clampZ);
        transform.position = Vector3.Lerp(transform.position, new Vector3(clampX, clampY, clampZ), 1);
    }
}
