using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClickEvents
{
    public string name;
    public MonoBehaviour script;
}
public class CustomCursor : MonoBehaviour
{
    [SerializeField] private RectTransform boundsBox;

    [SerializeField] private GameObject cursorPrefab;
    private GameObject myCursor;
    private RectTransform cursorUI; // Reference to the UI element you want to move
    private float cursorSpeed = 200f; // Speed at which the cursor moves
    //private Vector2 screenBounds; //= new Vector2(1920, 1080); // Screen bounds
    private float smoothingFactor = 3f;

    private bool buttonHeld = false;

    private Vector2 cursorMovement;

    [SerializeField] private Canvas myCanvas;

    [SerializeField] private ClickEvents[] clickEvent;

    Vector2 boundsX;
    Vector2 boundsY;

    private void Start()
    {
        myCanvas = FindObjectOfType<Canvas>();
        //screenBounds = new Vector2(Screen.width, Screen.height);
        myCursor = Instantiate(cursorPrefab, myCanvas.gameObject.GetComponent<RectTransform>());
        cursorUI = myCursor.GetComponent<RectTransform>();
        //screenBounds = cam.WorldToViewportPoint(cursorUI.position);
        boundsX = new Vector2(boundsBox.rect.xMin + boundsBox.position.x, boundsBox.rect.xMax + boundsBox.position.x);
        boundsY = new Vector2(boundsBox.rect.yMin + boundsBox.position.y, boundsBox.rect.yMax + boundsBox.position.y);
    }

    private void Update()
    {
        //cursorUI.anchoredPosition += cursorMovement * cursorSpeed;

        // Clamp cursor position to stay within camera bounds
        //Vector3 clampedPosition = cam.WorldToViewportPoint(cursorUI.position);
        //clampedPosition.x = Mathf.Clamp01(clampedPosition.x);
        //clampedPosition.y = Mathf.Clamp01(clampedPosition.y);
        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, cam.rect.x * Screen.width, (cam.rect.x + cam.rect.width) * Screen.width);
        //clampedPosition.y = Mathf.Clamp(clampedPosition.y, cam.rect.y * Screen.height, (cam.rect.y + cam.rect.height) * Screen.height);

        //cursorUI.position = cam.ViewportToWorldPoint(clampedPosition);
    }

    // Public function to move the cursor UI element
    public void MoveCursor(Vector2 input)
    {
        Vector3 currentPosition = cursorUI.position;

        // Calculate the new position based on input and speed
        //Vector3 newPosition = currentPosition + new Vector3(input.x, input.y) * cursorSpeed * Time.deltaTime;

        // Clamp the new position within screen bounds
        //newPosition.x = Mathf.Clamp(newPosition.x, 0, screenBounds.x);
        //newPosition.y = Mathf.Clamp(newPosition.y, 0, screenBounds.y);

        Vector3 targetMovement = new Vector3(input.x, input.y) * cursorSpeed;
        Vector3 targetPosition = currentPosition + targetMovement;

        // Smoothly move towards the target position
        //Vector3 clampedPosition = cam.WorldToViewportPoint(cursorUI.position);
        //clampedPosition.x = Mathf.Clamp01(clampedPosition.x);
        //clampedPosition.y = Mathf.Clamp01(clampedPosition.y);

        //Vector3 newPosition = new Vector3(Mathf.Clamp(targetPosition.x, 0, screenBounds.x), Mathf.Clamp(targetPosition.y, 0, screenBounds.y), currentPosition.z);
        Vector3 newPosition = new Vector3(Mathf.Clamp(targetPosition.x, boundsX.x, boundsX.y), Mathf.Clamp(targetPosition.y, boundsY.x, boundsY.y), currentPosition.z);
        Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, smoothingFactor * Time.deltaTime);



        // Update the UI element's position
        cursorUI.position = smoothedPosition;

        //cursorMovement = input;
    }

    public void Select() //instead of mouse click, uses controller button
    {
        CallEvent();
    }

    private void CallEvent()
    {
        for (int i = 0; i < clickEvent.Length; i++)
        {
            clickEvent[i].script.Invoke(clickEvent[i].name, 0f);
        }
    }

    public Vector3 GetCursorPos()
    {
        return cursorUI.position;
    }

    public void SetButtonHeld(bool held)
    {
        buttonHeld = held;
    }

    public bool GetButtonHeld()
    {
        return buttonHeld;
    }
}
