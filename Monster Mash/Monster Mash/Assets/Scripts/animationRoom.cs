using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationRoom : MonoBehaviour
{
    public GameObject safetyScreen;
    public GameObject floor;
    public GameObject groundedLimbCheck;
    public void removeSafetyScreen()
    {
        safetyScreen.SetActive(false);
    }

    public void removeFloor()
    {
        floor.SetActive(false);
    }

    public void bringBackFloor()
    {
        floor.SetActive(true);
    }

    public void removeGroundedLimbCheck()
    {
        groundedLimbCheck.SetActive(false);
    }
}
