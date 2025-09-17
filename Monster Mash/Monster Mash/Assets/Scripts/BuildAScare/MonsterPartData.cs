using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPartData
{
    public string partPrefabPath;

    public Quaternion partRotation;

    public Vector3 partScale;

    public Vector3 partPosition;

    public enum Button 
    { 
        NONE = 0, 
        BTTN_NORTH = 3, 
        BTTN_EAST = 1, 
        BTTN_WEST = 2, 
        R_BUMPER = 5, 
        L_BUMPER = 4, 
        R_TRIGGER = 7, 
        L_TRIGGER = 6,
    }

    public Button partButton;

    public List<int> palleteSwapIndex = new List<int>();

    public string partHexCode;

    public bool isFlipped = false;

}
