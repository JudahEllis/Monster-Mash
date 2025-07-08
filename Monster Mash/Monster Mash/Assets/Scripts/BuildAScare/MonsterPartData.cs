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

    public enum Button { NONE, BTTN_NORTH, BTTN_EAST, BTTN_WEST, R_BUMPER, L_BUMPER, R_TRIGGER, L_TRIGGER }

    public Button partButton;

    public List<int> palleteSwapIndex = new List<int>();

    public string partHexCode;

    public bool isFlipped = false;

}
