using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalableTimer : MonoBehaviour
{
    //public Image timerImage;            // The Image component of the circular timer
    [SerializeField] private RectTransform handTransform; // The RectTransform of the hand (pointer)

    [SerializeField] private Image circularImage;
    [SerializeField] private float duration = 60.0f; // Timer duration in seconds
    private float remainingTime;

    private void Start()
    {
        // Initialize the remaining time
        remainingTime = duration;
    }

    private void Update()
    {
        // Example: Update the timer progress (fillAmount) and rotate the hand
        /*float timerProgress = Mathf.PingPong(Time.time, 5.0f) / 5.0f; // Simulate timer progress
        timerImage.fillAmount = timerProgress;

        // Rotate the hand based on the timer progress (360 degrees for full circle)
        float handRotation = timerProgress * 360.0f;
        handTransform.localEulerAngles = new Vector3(0f, 0f, -handRotation);
        */
        // Update the remaining time
        remainingTime -= Time.deltaTime;

        // Update the fill amount based on the remaining time
        float fillAmount = Mathf.Clamp01(remainingTime / duration);
        circularImage.fillAmount = fillAmount;
        handTransform.localEulerAngles = new Vector3(0f, 0f, -fillAmount * 360);

        // If the timer reaches zero, perform an action (e.g., timer complete event)
        if (remainingTime <= 0)
        {
            TimerComplete();
        }
    }

    private void TimerComplete()
    {
        // Handle timer completion here (e.g., display a message)
        Debug.Log("Timer completed!");
    }

    public void ResetTimer()
    {
        remainingTime = duration;
    }
}
