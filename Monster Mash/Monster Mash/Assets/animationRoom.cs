using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationRoom : MonoBehaviour
{
    public GameObject safetyScreen;
    public void removeSafetyScreen()
    {
        safetyScreen.SetActive(false);
    }
}
