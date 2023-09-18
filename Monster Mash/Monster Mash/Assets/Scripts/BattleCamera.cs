using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private Transform[] players;

    [SerializeField] private float minZoom = 40f;  // Minimum camera zoom level.
    [SerializeField] private float maxZoom = 100f;  // Maximum camera zoom level.

    [SerializeField] private float minFOV = 60f;
    [SerializeField] private float maxFOV = 30f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float followSpeed = 5f;

    private Camera cam;
    public float padding = 1.0f;

    [SerializeField] private Transform stageBoundsMin; // The minimum position within the stage bounds.
    [SerializeField] private Transform stageBoundsMax; // The maximum position within the stage bounds.

    public float cameraLerpSpeed = 5f; // Adjust this value for the desired smoothing effect.

    private Vector3 cameraVelocity; // Stores the camera's velocity for smooth movement.

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] myPlayers = GameObject.FindGameObjectsWithTag("Player");

        players = new Transform[myPlayers.Length];

        for (int i = 0; i < myPlayers.Length; i++)
        {
            players[i] = myPlayers[i].transform;
        }

        cam = GetComponent<Camera>();

        if (cam.orthographic)
        {
            minZoom = 1f;
            maxZoom = 100f;
            padding = 2.5f;
        }
        else
        {
            minFOV = 60f;
            maxFOV = 30f;
            cam.fieldOfView = maxFOV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Length == 0)
            return;

        // Create a list to store players inside the stage bounds.
        List<Transform> playersInsideBounds = new List<Transform>();

        float maxDistance = 0f;

        // Calculate the desired orthographic size based on player distance and padding.
        Bounds playersBounds = new Bounds(Vector3.zero, Vector3.zero); // Initialize an empty bounds.

        foreach (Transform player in players)
        {
            if (IsInsideStageBounds(player.position))
            {
                playersInsideBounds.Add(player); // Add players inside the bounds to the list.
                playersBounds.Encapsulate(player.position); // Include players inside the bounds in the bounds calculation.

                foreach (Transform otherPlayer in players)
                {
                    if (IsInsideStageBounds(otherPlayer.position))
                    {
                        float distance = Vector3.Distance(player.position, otherPlayer.position);
                        maxDistance = Mathf.Max(maxDistance, distance);
                    }
                }
            }
        }

        float targetFOV = Mathf.Lerp(maxFOV, minFOV, maxDistance / 10f);

        // Encapsulate the stage bounds as max and min points.
        Bounds stageBounds = new Bounds((stageBoundsMin.position + stageBoundsMax.position) / 2f, stageBoundsMax.position - stageBoundsMin.position);

        // Calculate the camera's position within the stage bounds.
        float cameraX = Mathf.Clamp(playersBounds.center.x, stageBounds.min.x + cam.orthographicSize * cam.aspect, stageBounds.max.x - cam.orthographicSize * cam.aspect);
        float cameraY = Mathf.Clamp(playersBounds.center.y, stageBounds.min.y + cam.orthographicSize, stageBounds.max.y - cam.orthographicSize);

        Vector3 targetPosition = new Vector3(cameraX, cameraY, transform.position.z);

        if (playersInsideBounds.Count > 0)
        {
            // Zoom based on player bounds and padding for players inside the stage bounds.
            float targetOrthoSizeX = (playersBounds.size.x / 2f + padding) / cam.aspect;
            float targetOrthoSizeY = playersBounds.size.y / 2f + padding;
            float targetOrthoSize = Mathf.Clamp(Mathf.Max(targetOrthoSizeX, targetOrthoSizeY), minZoom, maxZoom);

            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, Time.deltaTime * cameraLerpSpeed);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
            }
        }

        // Apply smooth movement to camera position.
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, cameraLerpSpeed * Time.deltaTime);
    }

    bool IsInsideStageBounds(Vector3 position)
    {
        return position.x >= stageBoundsMin.position.x && position.x <= stageBoundsMax.position.x &&
               position.y >= stageBoundsMin.position.y && position.y <= stageBoundsMax.position.y;
    }

    /*public Transform[] players;
    public float minFOV = 30f;
    public float maxFOV = 60f;
    public float zoomSpeed = 2f;
    public float followSpeed = 5f;

    public float xPadding = 1.0f; // Horizontal padding.

    private Camera cam;

    private void Start()
    {
        GameObject[] myPlayers = GameObject.FindGameObjectsWithTag("Player");

        players = new Transform[myPlayers.Length];

        for (int i = 0; i < myPlayers.Length; i++)
        {
            players[i] = myPlayers[i].transform;
        }

        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (players.Length == 0)
            return;

        float maxDistance = 0f;

        foreach (Transform player in players)
        {
            foreach (Transform otherPlayer in players)
            {
                float distance = Vector3.Distance(player.position, otherPlayer.position);
                maxDistance = Mathf.Max(maxDistance, distance);
            }
        }

        // Calculate the desired FOV based on player distance and horizontal padding.
        float targetFOV = Mathf.Lerp(maxFOV, minFOV, maxDistance / 10f);

        // Calculate the horizontal FOV based on the vertical FOV and screen aspect ratio.
        float horizontalFOV = targetFOV * cam.aspect;

        // Calculate the horizontal padding in world units based on the horizontal FOV.
        float paddingX = maxDistance / Mathf.Tan(horizontalFOV * 0.5f * Mathf.Deg2Rad);

        // Apply the desired xPadding to the horizontal padding.
        float totalXPadding = paddingX + xPadding;

        // Calculate the vertical padding in world units based on the vertical FOV.
        float paddingY = maxDistance / Mathf.Tan(targetFOV * 0.5f * Mathf.Deg2Rad);

        // Apply the desired xPadding to the vertical padding.
        float totalYPadding = paddingY + xPadding;

        // Set the camera's field of view.
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

        Vector3 midpoint = Vector3.zero;

        foreach (Transform player in players)
        {
            midpoint += player.position;
        }

        midpoint /= players.Length;

        Vector3 cameraPosition = midpoint;
        cameraPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * followSpeed);
    }*/
}
