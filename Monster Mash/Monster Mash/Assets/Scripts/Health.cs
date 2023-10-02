using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int limbCount = 8;
    [SerializeField] private int currHealth;
    [SerializeField] private int currLimbCount;

    public Slider healthSlider;
    private bool isThereASlider = false;

    [SerializeField] private int singleLimbHealth = 20;

    [SerializeField] private int currLimbHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = limbCount * singleLimbHealth;

        currHealth = maxHealth;
        currLimbCount = limbCount;

        currLimbHealth = singleLimbHealth;

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
        currHealth = Mathf.RoundToInt(Mathf.Max(0f, currHealth - loseValue));


        // Update the current limb's health
        currLimbHealth = Mathf.Max(0, currLimbHealth - loseValue);

        // Check if the current limb has lost all its health
        if (currLimbHealth == 0)
        {
            currLimbHealth = singleLimbHealth;
            // A limb was lost, call the function
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
