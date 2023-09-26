using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int limbCount = 8;
    [SerializeField] private int currHealth;
    [SerializeField] private int segmentValue;
    [SerializeField] private int currSegment;
    [SerializeField] private int currLimbCount;

    public Slider healthSlider;
    private bool isThereASlider = false;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        segmentValue = Mathf.CeilToInt((float) maxHealth / limbCount);
        currSegment = limbCount;
        currLimbCount = limbCount;

        if (healthSlider != null)
        {
            isThereASlider = true;

            healthSlider.maxValue = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isThereASlider)
        {
            healthSlider.value = currHealth;
        }

        if (currHealth <= 0)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoseHealth(1);
        }
    }

    public void LoseHealth(int loseValue)
    {
        // Calculate the remaining health within the current segment
        int healthWithinSegment = currHealth % segmentValue;

        // Subtract the health loss
        currHealth = Mathf.CeilToInt(Mathf.Max(0f, currHealth - loseValue));

        // Calculate the remaining health within the new current segment
        int newHealthWithinSegment = currHealth % segmentValue;

        // Check if a segment was fully depleted
        if (newHealthWithinSegment > healthWithinSegment)
        {
            // A segment was lost, call the function
            PopThatLimbOff();
        }
    }

    public void GainHealth(int gainValue)
    {
        currHealth = Mathf.Min(maxHealth, currHealth + gainValue);
    }

    private void PopThatLimbOff()
    {
        currLimbCount -= 1;
        Debug.Log("Goodbye limb #" + currLimbCount + " out of " + limbCount + "!");
    }

    private void Death()
    {
        Debug.Log("u died scrub");
    }
}
