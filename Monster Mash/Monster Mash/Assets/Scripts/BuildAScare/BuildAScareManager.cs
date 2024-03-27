using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAScareManager : MonoBehaviour
{
    PlayerInput input;

    void Awake()
    {
        input = FindObjectOfType<PlayerInput>();

        input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part").Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
