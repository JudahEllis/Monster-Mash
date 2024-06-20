using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UndoData
{
    public MonsterPartData partData;

    public GameObject monsterObj;

    public UndoData(Vector3 position, Vector3 scale, Quaternion rotation, GameObject obj)
    {
        partData = new MonsterPartData();

        partData.partPosition = position;

        partData.partScale = scale;

        partData.partRotation = rotation;

        monsterObj = obj;
    }
}
