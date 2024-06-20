using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPartData : MonoBehaviour
{
    public MonsterPartData monsterPart = new MonsterPartData();

    public void SavePartData()
    {
        monsterPart.partRotation = transform.localRotation;

        monsterPart.partScale = transform.localScale;

        monsterPart.partPosition = transform.localPosition;
    }
}
